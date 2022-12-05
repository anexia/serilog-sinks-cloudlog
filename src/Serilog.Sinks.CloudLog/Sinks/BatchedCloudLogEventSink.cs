using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Anexia.BDP.CloudLog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.CloudLog.Sinks.Formatting;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.CloudLog.Sinks
{
    public sealed class BatchedCloudLogEventSink : IBatchedLogEventSink, IDisposable
    {
        private readonly Client _client;
        private readonly CloudLogJsonFormatter _formatter;

        public BatchedCloudLogEventSink(string index, string token, HttpClient client)
        {
            try
            {
                _client = new Client(index, token, client);
                _formatter = new CloudLogJsonFormatter();
                Init();
            }
            catch (Exception exception)
            {
                SelfLog.WriteLine("Unable to create CloudLogSink for index {0}: {1}", index, exception);
            }
        }

        public Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var task = default(Task);
            foreach (var e in events)
            {
                var sw = new StringWriter();
                _formatter.Format(e, sw);
                if (task != null)
                    task = task.ContinueWith(
                        (_, swInner) => _client.PushEvent(swInner.ToString()),
                        sw,
                        TaskScheduler.Current);
                else
                    task = _client.PushEvent(sw.ToString());
            }

            return task;
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private void Init()
        {
            _client.SetClientType("dotnet-client-serilog");
        }
    }
}
