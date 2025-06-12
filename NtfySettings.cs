using Serilog.Events;

namespace Logrus.Serilog.Ntfy;

public class NtfySettings
{
  public required string Endpoint { get; init; }

  public required string Username { get; init; }

  public required string Password { get; init; }

  public required string Topic { get; init; }

  public required string Title { get; init; }

  public required string[] Tags { get; init; }

  public required LogEventLevel MinimumLevel { get; init; }
}
