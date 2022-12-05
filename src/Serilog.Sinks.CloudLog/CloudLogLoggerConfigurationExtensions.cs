using System;
using System.Net.Http;
using Serilog.Configuration;
using Serilog.Sinks.CloudLog.Sinks;

namespace Serilog
{
    /// <summary>
    ///     Adds the WriteTo.CloudLog() extension method to <see cref="LoggerConfiguration" />.
    /// </summary>
    public static class CloudLogLoggerConfigurationExtensions
    {
        /// <summary>
        /// Adds a sink that writes log events to a CloudLog index.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="index">The index name.</param>
        /// <param name="token">The CloudLog index token.</param>
        /// <returns><see cref="LoggerConfiguration"/></returns>
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
        
        /// <summary>
        /// Adds a sink that writes log events to a CloudLog index.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="index">The index name.</param>
        /// <param name="token">The CloudLog index token.</param>
        /// <param name="createClient">Function to create a client, parameter is the name of the class that uses</param>
        /// <returns><see cref="LoggerConfiguration"/></returns>
        public static LoggerConfiguration CloudLog(
            this LoggerSinkConfiguration loggerConfiguration,
            string index,
            string token,
            Func<string, HttpClient> createClient)
        {
            var sink = new CloudLogSink(
                index,
                token,
                createClient);
            return loggerConfiguration.Sink(sink);
        }
        
        /// <summary>
        /// Adds a sink that writes log events to a CloudLog index.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="index">The index name.</param>
        /// <param name="token">The CloudLog index token.</param>
        /// <param name="clientFactory">Client factory to create Http clients, it creates without naming the clients</param>
        /// <returns><see cref="LoggerConfiguration"/></returns>
        public static LoggerConfiguration CloudLog(
            this LoggerSinkConfiguration loggerConfiguration,
            string index,
            string token,
            IHttpClientFactory clientFactory)
        {
            var sink = new CloudLogSink(
                index,
                token,
                clientFactory);
            return loggerConfiguration.Sink(sink);
        }
    }
}
