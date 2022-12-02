using System;
using System.IO;
using System.Linq;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using Serilog.Parsing;

namespace Serilog.Sinks.CloudLog.Formatting
{
    class CloudLogJsonFormatter : ITextFormatter
    {
        readonly JsonValueFormatter _valueFormatter;

        /// <summary>
        /// Construct a <see cref="CloudLogJsonFormatter"/>, optionally supplying a formatter for
        /// <see cref="LogEventPropertyValue"/>s on the event.
        /// </summary>
        /// <param name="valueFormatter">A value formatter, or null.</param>
        public CloudLogJsonFormatter(JsonValueFormatter valueFormatter = null)
        {
            _valueFormatter = valueFormatter ?? new JsonValueFormatter(typeTagName: "$type");
        }

        /// <summary>
        /// Format the log event into the output. Subsequent events will be newline-delimited.
        /// </summary>
        /// <param name="logEvent">The event to format.</param>
        /// <param name="output">The output.</param>
        public void Format(LogEvent logEvent, TextWriter output)
        {
            FormatEvent(logEvent, output, _valueFormatter);
            output.WriteLine();
        }

        /// <summary>
        /// Format the log event into the output.
        /// </summary>
        /// <param name="logEvent">The event to format.</param>
        /// <param name="output">The output.</param>
        /// <param name="valueFormatter">A value formatter for <see cref="LogEventPropertyValue"/>s on the event.</param>
        public static void FormatEvent(LogEvent logEvent, TextWriter output, JsonValueFormatter valueFormatter)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (valueFormatter == null) throw new ArgumentNullException(nameof(valueFormatter));

            output.Write("{\"timestamp\":");
            output.Write((long)logEvent.Timestamp.UtcDateTime.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds);
            output.Write(",\"template\":");
            JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Text, output);
            output.Write(",\"message\":");
            JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Render(logEvent.Properties), output);

            var tokensWithFormat = logEvent.MessageTemplate.Tokens
                .OfType<PropertyToken>()
                .Where(pt => pt.Format != null);

            if (logEvent.Level != LogEventLevel.Information)
            {
                output.Write(",\"level\":\"");
                output.Write(logEvent.Level);
                output.Write('\"');
            }

            if (logEvent.Exception != null)
            {
                output.Write(",\"exception\":");
                JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
            }


            if (logEvent.Properties.Count != 0)
            {
                output.Write(",\"properties\":{");
                var precedingDelimiter = "";
                foreach (var property in logEvent.Properties)
                {
                    output.Write(precedingDelimiter);
                    precedingDelimiter = ",";
                    JsonValueFormatter.WriteQuotedJsonString(property.Key, output);
                    output.Write(':');
                    valueFormatter.Format(property.Value, output);
                }

                output.Write('}');
            }

            output.Write('}');
        }

    }
}
