using Platform.Domain.Common;

namespace Platform.Domain.Entities;

public sealed class TenantModule : ITenantEntity
{
    private TenantModule()
    {
    }

    private TenantModule(Guid tenantId, Guid moduleId, DateTimeOffset installedAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        ModuleId = moduleId;
        IsEnabled = true;
        InstalledAt = installedAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid ModuleId { get; private set; }
    public bool IsEnabled { get; private set; }
    public DateTimeOffset InstalledAt { get; private set; }

    public static TenantModule Install(Guid tenantId, Guid moduleId, DateTimeOffset installedAt)
    {
        return new TenantModule(tenantId, moduleId, installedAt);
    }
}
