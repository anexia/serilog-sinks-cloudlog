using System;
using System.Collections.Generic;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.PeriodicBatching;
using Anexia.BDP.CloudLog;
using System.IO;

namespace Serilog.Sinks.CloudLog
{
    public class CloudLogSink : PeriodicBatchingSink
    {
        private Client client;
        private JsonFormatter formatter;

        const int batchSize = 50;
        const int period = 1;

        public CloudLogSink(
            string index,
            string caFile,
            string certFile,
            string keyFile) : base(batchSize, TimeSpan.FromSeconds(period))
        {
            this.client = new Client(index, caFile, certFile, keyFile);
            this.formatter = new JsonFormatter(closingDelimiter: null, renderMessage: true);
        }

        public CloudLogSink(
            string index,
            string token) : base(batchSize, TimeSpan.FromSeconds(period))
        {
            this.client = new Client(index, token);
            this.formatter = new JsonFormatter(closingDelimiter: null, renderMessage: true);
        }

        protected override void EmitBatch(IEnumerable<LogEvent> events)
        {
            foreach (var e in events)
            {
                var sw = new StringWriter();
                formatter.Format(e, sw);
                client.PushEvent(sw.ToString());
            }
        }
    }
}
