using System.Net.Http.Headers;
using System.Text;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Http.HttpClients;

namespace Logrus.Serilog.Ntfy;

public static class NtfyHttpSinkExtensions
{
    public static LoggerConfiguration NtfyHttp(
        this LoggerSinkConfiguration loggerConfiguration,
        string ntfyEndpoint,
        string ntfyUsername,
        string ntfyPassword,
        string ntfyTopic,
        string ntfyTitle,
        string[] ntfyTags,
        LogEventLevel restrictedToMinimumLevel
        )
    {
        var authenticationString = $"{ntfyUsername}:{ntfyPassword}";
        var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
        return loggerConfiguration.Http(
            requestUri: ntfyEndpoint,
            httpClient: new JsonHttpClient(httpClient),
            queueLimitBytes: null,
            textFormatter: new NtfyLogFormatter(),
            batchFormatter: new NtfyBatchFormatter(ntfyTopic, ntfyTitle, ntfyTags),
            restrictedToMinimumLevel: restrictedToMinimumLevel);
    }
}
