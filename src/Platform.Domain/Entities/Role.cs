using Platform.Domain.Common;

namespace Platform.Domain.Entities;

public sealed class Role : ITenantEntity
{
    private Role()
    {
    }

    private Role(Guid tenantId, string name, bool isSystemRole)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        IsSystemRole = isSystemRole;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsSystemRole { get; private set; }

    public static Role Create(Guid tenantId, string name, bool isSystemRole)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Role(tenantId, name, isSystemRole);
    }
}
