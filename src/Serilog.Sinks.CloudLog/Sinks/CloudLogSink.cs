using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Anexia.BDP.CloudLog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.CloudLog.Sinks.Formatting;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.CloudLog.Sinks
{
    public class CloudLogSink : PeriodicBatchingSink
    {
        private const int BatchSize = 50;
        private const int Period = 1;

        private readonly Client _client;
        private readonly CloudLogJsonFormatter _formatter;

        public CloudLogSink(string index, string token, Func<string, HttpClient> createClient)
            : base(new BatchedCloudLogEventSink(index, token, createClient(nameof(BatchedCloudLogEventSink))), new PeriodicBatchingSinkOptions()
            {
                BatchSizeLimit = BatchSize,
                Period =  TimeSpan.FromSeconds(Period),
            })
        {
            try
            {
                _client = new Client(index, token, createClient(nameof(CloudLogSink)));
                _formatter = new CloudLogJsonFormatter();
                Init();
            }
            catch (Exception exception)
            {
                SelfLog.WriteLine("Unable to create CloudLogSink for index {0}: {1}", index, exception);
            }
        }

        public CloudLogSink(string index, string token)
            : this(index, token, (_) => new HttpClient())
        {
        }
        
        public CloudLogSink(string index, string token, IHttpClientFactory clientFactory)
            : this(index, token, clientFactory.CreateClient)
        {
        }

        private void Init()
        {
            _client.SetClientType("dotnet-client-serilog");
        }

        protected override void EmitBatch(IEnumerable<LogEvent> events)
        {
            foreach (var e in events)
            {
                var sw = new StringWriter();
                _formatter.Format(e, sw);
                _client.PushEvent(sw.ToString());
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _client.Dispose();
        }
    }
}
