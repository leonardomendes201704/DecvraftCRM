using Microsoft.EntityFrameworkCore;
using Platform.Domain.Catalog;
using Platform.Domain.Entities;
using Platform.Persistence;
using Platform.Provisioning.Abstractions;
using Platform.Provisioning.Models;

namespace Platform.Provisioning.Services;

public sealed class InstallerLockService : IInstallerLockService
{
    private readonly AppDbContext _dbContext;

    public InstallerLockService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InstallStatusResult> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        var installation = await GetInstallationAsync(cancellationToken);

        return new InstallStatusResult
        {
            IsInstalled = installation?.IsInstalled ?? false,
            Version = installation?.IsInstalled == true ? installation.InstalledVersion : null,
            InstalledAt = installation?.InstalledAt
        };
    }

    public async Task<bool> IsInstalledAsync(CancellationToken cancellationToken = default)
    {
        var installation = await GetInstallationAsync(cancellationToken);

        return installation?.IsInstalled ?? false;
    }

    public async Task LockAsync(string installedVersion, string installedBy, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(installedVersion);
        ArgumentException.ThrowIfNullOrWhiteSpace(installedBy);

        var installation = await GetInstallationAsync(cancellationToken);

        if (installation is null)
        {
            installation = SystemInstallation.CreatePending(
                InstallerDefaults.InstallationKey,
                installedVersion,
                InstallerDefaults.Environment);

            _dbContext.SystemInstallations.Add(installation);
        }

        installation.MarkInstalled(installedVersion, installedBy, DateTimeOffset.UtcNow);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private Task<SystemInstallation?> GetInstallationAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SystemInstallations
            .SingleOrDefaultAsync(installation => installation.InstallationKey == InstallerDefaults.InstallationKey, cancellationToken);
    }
}
