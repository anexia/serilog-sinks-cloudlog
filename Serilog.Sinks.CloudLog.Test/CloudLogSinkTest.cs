using System;
using Serilog.Events;
using Xunit;

namespace Serilog.Sinks.CloudLog.Test
{
    public class CloudLogSinkTest
    {
        private static readonly LogEvent _testLogEvent = new(DateTimeOffset.Now, LogEventLevel.Debug, null,
            MessageTemplate.Empty,
            ArraySegment<LogEventProperty>.Empty);

        [Fact]
        public void CloudLog()
        {
            var sink = new CloudLogSink(
                "SomeIndex",
                "SomeToken");
            sink.Emit(_testLogEvent);
        }

        [Fact]
        public void CloudLogException()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                var cloudsink = new CloudLogSink(
                    "",
                    "");
                cloudsink.Emit(_testLogEvent);
            });
        }
    }
}