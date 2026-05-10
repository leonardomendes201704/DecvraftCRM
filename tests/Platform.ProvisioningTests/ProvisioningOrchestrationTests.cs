using Platform.Domain.Catalog;
using Platform.Provisioning.Abstractions;
using Platform.Provisioning.Models;
using Platform.Provisioning.Services;
using Platform.Provisioning.Validation;

namespace Platform.ProvisioningTests;

public sealed class ProvisioningOrchestrationTests
{
    private const string DatabaseHost = "mssql";
    private const string DatabaseName = "WhiteLabelErp";
    private const string DatabaseUser = "sa";
    private const string DatabasePassword = "bootstrap-only";
    private const string CompanyName = "Empresa Demo";
    private const string TenantSlug = "empresa-demo";
    private const string AdminName = "Administrador";
    private const string AdminEmail = "admin@demo.com";
    private const string AdminPassword = "Admin@123456";
    private const string SystemName = "ERP Demo";

    [Fact]
    public async Task InstallAsync_ShouldReturnValidationFailureWithoutRunningSteps()
    {
        var recorder = new ProvisioningCallRecorder();
        var service = CreateService(recorder);

        var result = await service.InstallAsync(new InstallRequest());

        Assert.False(result.Succeeded);
        Assert.Contains(ProvisioningErrors.DatabaseHostRequired, result.Errors);
        Assert.Empty(recorder.Calls);
    }

    [Fact]
    public async Task InstallAsync_ShouldReturnFailureWhenAlreadyInstalled()
    {
        var recorder = new ProvisioningCallRecorder { IsInstalled = true };
        var service = CreateService(recorder);

        var result = await service.InstallAsync(CreateValidRequest());

        Assert.False(result.Succeeded);
        Assert.Contains(ProvisioningErrors.AlreadyInstalled, result.Errors);
        Assert.Equal(
            [
                ProvisioningCallNames.TestConnection,
                ProvisioningCallNames.EnsureDatabaseCreated,
                ProvisioningCallNames.SwitchConnection,
                ProvisioningCallNames.IsInstalled
            ],
            recorder.Calls);
    }

    [Fact]
    public async Task InstallAsync_ShouldRunProvisioningStepsInOrder()
    {
        var recorder = new ProvisioningCallRecorder();
        var service = CreateService(recorder);

        var result = await service.InstallAsync(CreateValidRequest());

        Assert.True(result.Succeeded);
        Assert.Equal(recorder.TenantId, result.TenantId);
        Assert.Equal(recorder.AdminUserId, result.AdminUserId);
        Assert.Equal(KnownModules.DefaultSlugs, result.InstalledModules);
        Assert.Equal(
            [
                ProvisioningCallNames.TestConnection,
                ProvisioningCallNames.EnsureDatabaseCreated,
                ProvisioningCallNames.SwitchConnection,
                ProvisioningCallNames.IsInstalled,
                ProvisioningCallNames.RunMigrations,
                ProvisioningCallNames.RunSeeds,
                ProvisioningCallNames.CreateTenant,
                ProvisioningCallNames.CreateBranding,
                ProvisioningCallNames.CreateAdminUser,
                ProvisioningCallNames.InstallModules,
                ProvisioningCallNames.LockInstaller
            ],
            recorder.Calls);
    }

    private static ProvisioningService CreateService(ProvisioningCallRecorder recorder)
    {
        return new ProvisioningService(
            new FakeDatabaseProvisioner(recorder),
            new FakeConnectionSwitcher(recorder),
            new FakeMigrationRunner(recorder),
            new FakeSeedRunner(recorder),
            new FakeTenantProvisioner(recorder),
            new FakeModuleInstaller(recorder),
            new FakeInstallerLockService(recorder));
    }

    private static InstallRequest CreateValidRequest()
    {
        return new InstallRequest
        {
            Database = new DatabaseSetupOptions
            {
                Host = DatabaseHost,
                DatabaseName = DatabaseName,
                Username = DatabaseUser,
                Password = DatabasePassword
            },
            Tenant = new TenantSetupOptions
            {
                CompanyName = CompanyName,
                Slug = TenantSlug
            },
            AdminUser = new AdminUserSetupOptions
            {
                Name = AdminName,
                Email = AdminEmail,
                Password = AdminPassword
            },
            Branding = new BrandingSetupOptions
            {
                SystemName = SystemName
            }
        };
    }

    private sealed class ProvisioningCallRecorder
    {
        public Guid TenantId { get; } = Guid.NewGuid();
        public Guid BrandingId { get; } = Guid.NewGuid();
        public Guid AdminUserId { get; } = Guid.NewGuid();
        public bool IsInstalled { get; init; }
        public List<string> Calls { get; } = [];
    }

    private static class ProvisioningCallNames
    {
        public const string IsInstalled = "IsInstalled";
        public const string TestConnection = "TestConnection";
        public const string EnsureDatabaseCreated = "EnsureDatabaseCreated";
        public const string SwitchConnection = "SwitchConnection";
        public const string RunMigrations = "RunMigrations";
        public const string RunSeeds = "RunSeeds";
        public const string CreateTenant = "CreateTenant";
        public const string CreateBranding = "CreateBranding";
        public const string CreateAdminUser = "CreateAdminUser";
        public const string InstallModules = "InstallModules";
        public const string LockInstaller = "LockInstaller";
    }

    private sealed class FakeDatabaseProvisioner : IDatabaseProvisioner
    {
        private readonly ProvisioningCallRecorder _recorder;

        public FakeDatabaseProvisioner(ProvisioningCallRecorder recorder)
        {
            _recorder = recorder;
        }

        public Task TestConnectionAsync(DatabaseSetupOptions options, CancellationToken cancellationToken = default)
        {
            _recorder.Calls.Add(ProvisioningCallNames.TestConnection);
            return Task.CompletedTask;
        }

        public Task EnsureDatabaseCreatedAsync(DatabaseSetupOptions options, CancellationToken cancellationToken = default)
        {
            _recorder.Calls.Add(ProvisioningCallNames.EnsureDatabaseCreated);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeConnectionSwitcher : IProvisioningDbConnectionSwitcher
    {
        private readonly ProvisioningCallRecorder _recorder;

        public FakeConnectionSwitcher(ProvisioningCallRecorder recorder)
        {
            _recorder = recorder;
        }

        public Task UseTargetDatabaseAsync(DatabaseSetupOptions options, CancellationToken cancellationToken = default)
        {
            _recorder.Calls.Add(ProvisioningCallNames.SwitchConnection);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeMigrationRunner : IMigrationRunner
    {
        private readonly ProvisioningCallRecorder _recorder;

        public FakeMigrationRunner(ProvisioningCallRecorder recorder)
        {
            _recorder = recorder;
        }

        public Task RunAsync(CancellationToken cancellationToken = default)
        {
            _recorder.Calls.Add(ProvisioningCallNames.RunMigrations);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<string>> GetPendingMigrationsAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<string>>([]);
        }
    }

    private sealed class FakeSeedRunner : ISeedRunner
    {
        private readonly ProvisioningCallRecorder _recorder;

        public FakeSeedRunner(ProvisioningCallRecorder recorder)
        {
            _recorder = recorder;
        }

        public Task RunAsync(CancellationToken cancellationToken = default)
        {
            _recorder.Calls.Add(ProvisioningCallNames.RunSeeds);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeTenantProvisioner : ITenantProvisioner
    {
        private readonly ProvisioningCallRecorder _recorder;

        public FakeTenantProvisioner(ProvisioningCallRecorder recorder)
        {
            _recorder = recorder;
        }

        public Task<Guid> CreateTenantAsync(TenantSetupOptions options, CancellationToken cancellationToken = default)
        {
            _recorder.Calls.Add(ProvisioningCallNames.CreateTenant);
            return Task.FromResult(_recorder.TenantId);
        }

        public Task<Guid> CreateBrandingAsync(Guid tenantId, BrandingSetupOptions options, CancellationToken cancellationToken = default)
        {
            _recorder.Calls.Add(ProvisioningCallNames.CreateBranding);
            return Task.FromResult(_recorder.BrandingId);
        }

        public Task<Guid> CreateAdminUserAsync(Guid tenantId, AdminUserSetupOptions options, CancellationToken cancellationToken = default)
        {
            _recorder.Calls.Add(ProvisioningCallNames.CreateAdminUser);
            return Task.FromResult(_recorder.AdminUserId);
        }
    }

    private sealed class FakeModuleInstaller : IModuleInstaller
    {
        private readonly ProvisioningCallRecorder _recorder;

        public FakeModuleInstaller(ProvisioningCallRecorder recorder)
        {
            _recorder = recorder;
        }

        public Task InstallBaseModulesAsync(Guid tenantId, IReadOnlyCollection<string> moduleSlugs, CancellationToken cancellationToken = default)
        {
            _recorder.Calls.Add(ProvisioningCallNames.InstallModules);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeInstallerLockService : IInstallerLockService
    {
        private readonly ProvisioningCallRecorder _recorder;

        public FakeInstallerLockService(ProvisioningCallRecorder recorder)
        {
            _recorder = recorder;
        }

        public Task<InstallStatusResult> GetStatusAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new InstallStatusResult { IsInstalled = _recorder.IsInstalled });
        }

        public Task<bool> IsInstalledAsync(CancellationToken cancellationToken = default)
        {
            _recorder.Calls.Add(ProvisioningCallNames.IsInstalled);
            return Task.FromResult(_recorder.IsInstalled);
        }

        public Task LockAsync(string installedVersion, string installedBy, CancellationToken cancellationToken = default)
        {
            _recorder.Calls.Add(ProvisioningCallNames.LockInstaller);
            return Task.CompletedTask;
        }
    }
}
