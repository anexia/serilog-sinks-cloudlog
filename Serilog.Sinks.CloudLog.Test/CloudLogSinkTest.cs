using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Anexia.BDP.CloudLog;
using Anexia.BDP.CloudLog.Models;
using Moq;
using Moq.Protected;
using Serilog.Events;
using Serilog.Sinks.CloudLog.Sinks;
using Xunit;

namespace Serilog.Sinks.CloudLog.Test
{
    public class CloudLogSinkTest
    {
        private const string TestContent = "test content";

        private CloudLogSink CreateCloudLog(Action<HttpRequestMessage, CancellationToken> callbackFunc, bool createFail = false)
        {
            var moqHttpMessageHandler = new Mock<HttpMessageHandler>();
            moqHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback(callbackFunc)
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(TestContent)
                });
            if (createFail)
            {
                return new CloudLogSink("", "", (_) => new HttpClient(moqHttpMessageHandler.Object));
            }
            else
            {
                return new CloudLogSink("SomeIndex", "SomeToken", (_) => new HttpClient(moqHttpMessageHandler.Object));
            }

        }

        private static readonly LogEvent _testLogEvent = new(DateTimeOffset.Now, LogEventLevel.Debug, null,
            MessageTemplate.Empty,
            ArraySegment<LogEventProperty>.Empty);

        [Fact]
        public async Task CloudLog()
        {
            var result = default(PushEventPayload);
            var sink = CreateCloudLog((msg, cancel) =>
            {
                result = msg.Content.ReadFromJsonAsync<PushEventPayload>().Result;
            });
            sink.Emit(_testLogEvent);
            while (result == null)
            {
                await Task.Delay(10);
            }

            Assert.Contains("message", result.Records[0].Keys);
            Assert.Contains("timestamp", result.Records[0].Keys);
            Assert.Contains("cloudlog_client_type", result.Records[0].Keys);
            Assert.Contains("cloudlog_source_host", result.Records[0].Keys);
            Assert.True(result.Records[0].Keys.Count() >= 4);
        }

        [Fact]
        public async Task CloudLogException()
        {
            var result = default(PushEventPayload);
            var sink = CreateCloudLog((msg, cancel) =>
            {
                result = msg.Content.ReadFromJsonAsync<PushEventPayload>().Result;
            }, true);
            sink.Emit(_testLogEvent);
            await Task.Delay(100);

            Assert.Null(result);
        }

        [Fact]
        public async Task CloudLogMultiple()
        {
            var counter = 100;
            var result = 0;
            var sink = CreateCloudLog((msg, cancel) =>
            {
                var resultObject = msg.Content.ReadFromJsonAsync<PushEventPayload>().Result;
                if (resultObject != null)
                {
                    result++;
                }
            });

            for (int i = 0; i < counter; i++)
            {
                sink.Emit(_testLogEvent);
            }
            var cancellationTokenSource = new CancellationTokenSource(20000);

            while (result < counter)
            {
                await Task.Delay(10,cancellationTokenSource.Token);
            }

            Assert.Equal(100, result);
        }
    }
}
