using Serilog.Configuration;
using Serilog.Sinks.CloudLog;

namespace Serilog
{
    /// <summary>
    ///     Adds the WriteTo.CloudLog() extension method to <see cref="LoggerConfiguration" />.
    /// </summary>
    public static class LoggerConfigurationKafkaExtensions
    {
        /// <summary>
        /// Adds a sink that writes log events to a CloudLog index (HTTP interface).
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="index">The index name.</param>
        /// <param name="token">The CloudLog index token.</param>
        /// <returns></returns>
        public static LoggerConfiguration CloudLog(
            this LoggerSinkConfiguration loggerConfiguration,
            string index,
            string token)
        {
            var sink = new CloudLogSink(
                index,
                token);
            return loggerConfiguration.Sink(sink);
        }
    }
}
