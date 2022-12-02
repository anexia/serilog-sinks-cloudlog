using System;
using System.Collections.Generic;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Debugging;
using Anexia.BDP.CloudLog;
using System.IO;
using Serilog.Sinks.CloudLog.Formatting;

namespace Serilog.Sinks.CloudLog
{
    public class CloudLogSink : PeriodicBatchingSink
    {
        private const int BatchSize = 50;
        private const int Period = 1;

        private readonly Client _client;
        private readonly CloudLogJsonFormatter _formatter;

        public CloudLogSink(string index, string token)
            : base(new BatchedCloudLogEventSink(index, token), new PeriodicBatchingSinkOptions()
            {
                BatchSizeLimit = BatchSize,
                QueueLimit = Period
            })
        {
            try
            {
                _client = new Client(index, token);
                _formatter = new CloudLogJsonFormatter();
                Init();
            }
            catch (Exception exception)
            {
                SelfLog.WriteLine("Unable to create CloudLogSink for index {0}: {1}", index, exception);
            }
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