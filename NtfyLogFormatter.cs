using Serilog.Events;
using Serilog.Formatting;

namespace Logrus.Serilog.Ntfy;

public class NtfyLogFormatter: ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        if (logEvent.Exception == null)
        {
            output.WriteLine(logEvent.RenderMessage());
        }
        else if (logEvent.Exception == logEvent.Exception.GetBaseException())
        {
            output.WriteLine(logEvent.Exception.Message);
        }
        else
        {
            output.WriteLine($"{logEvent.Exception.Message} | {logEvent.Exception.GetBaseException().Message}");
        }
    }
}
