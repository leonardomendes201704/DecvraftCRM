namespace Platform.Domain.Entities;

public sealed class SystemInstallation
{
    private SystemInstallation()
    {
    }

    private SystemInstallation(string installationKey, string installedVersion, string environment)
    {
        Id = Guid.NewGuid();
        InstallationKey = installationKey;
        InstalledVersion = installedVersion;
        Environment = environment;
        IsInstalled = false;
    }

    public Guid Id { get; private set; }
    public string InstallationKey { get; private set; } = string.Empty;
    public string InstalledVersion { get; private set; } = string.Empty;
    public bool IsInstalled { get; private set; }
    public DateTimeOffset? InstalledAt { get; private set; }
    public string? InstalledBy { get; private set; }
    public string Environment { get; private set; } = string.Empty;

    public static SystemInstallation CreatePending(string installationKey, string installedVersion, string environment)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(installationKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(installedVersion);
        ArgumentException.ThrowIfNullOrWhiteSpace(environment);

        return new SystemInstallation(installationKey, installedVersion, environment);
    }

    public void MarkInstalled(string installedVersion, string installedBy, DateTimeOffset installedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(installedVersion);
        ArgumentException.ThrowIfNullOrWhiteSpace(installedBy);

        InstalledVersion = installedVersion;
        InstalledBy = installedBy;
        InstalledAt = installedAt;
        IsInstalled = true;
    }
}
