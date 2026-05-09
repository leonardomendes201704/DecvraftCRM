namespace Platform.Provisioning.Models;

public sealed class InstallStatusResult
{
    public bool IsInstalled { get; init; }
    public string? Version { get; init; }
    public DateTimeOffset? InstalledAt { get; init; }
}
