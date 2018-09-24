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
        /// Adds a sink that writes log events to a CloudLog index.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="index">The index name.</param>
        /// <param name="caFile">The path of the CloudLog CA certificate.</param>
        /// <param name="certFile">The path of the CloudLog client certificate.</param>
        /// <param name="keyFile">The path of the CloudLog client key.</param>
        /// <returns></returns>
        public static LoggerConfiguration CloudLog(
            this LoggerSinkConfiguration loggerConfiguration,
            string index,
            string caFile,
            string certFile,
            string keyFile,
            string keyPassword="")
        {
            var sink = new CloudLogSink(
                index,
                caFile,
                certFile,
                keyFile,
                keyPassword);
            return loggerConfiguration.Sink(sink);
        }

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
