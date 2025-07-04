using System.Text.Json;
using Serilog.Sinks.Http;

namespace Logrus.Serilog.Ntfy;

public class NtfyBatchFormatter(string topic, string title, string[] tags, int maxLength) : IBatchFormatter
{
    public void Format(IEnumerable<string> logEvents, TextWriter output)
    {
        var entry = new
        {
            topic,
            message = string.Join("\n", logEvents.Select(x => $"\u2757 {x}")) switch
            {
                var msg when msg.Length <= maxLength => msg,
                var msg => msg[..maxLength]
            },
            title,
            tags,
        };
        output.WriteLine(JsonSerializer.Serialize(entry));
    }
}