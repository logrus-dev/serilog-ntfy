using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
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
        LogEventLevel restrictedToMinimumLevel,
        int maxLength = 5000
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
            batchFormatter: new NtfyBatchFormatter(ntfyTopic, ntfyTitle, ntfyTags, maxLength),
            restrictedToMinimumLevel: restrictedToMinimumLevel);
    }

    public static void NtfyHttp(
        this LoggerSinkConfiguration loggerConfiguration,
        IConfiguration configuration)
      {
        var section = configuration.GetSection("Logging:Ntfy");
        if (!section.Exists()) return;
        var ntfySettings = section.Get<NtfySettings>();
        if (ntfySettings == null) return;
        loggerConfiguration.NtfyHttp(ntfySettings.Endpoint, ntfySettings.Username, ntfySettings.Password, ntfySettings.Topic, ntfySettings.Title, ntfySettings.Tags, ntfySettings.MinimumLevel);
      }
}
