using Platform.Domain.Common;

namespace Platform.Domain.Entities;

public sealed class RolePermission : ITenantEntity
{
    private RolePermission()
    {
    }

    private RolePermission(Guid tenantId, Guid roleId, Guid permissionId)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        RoleId = roleId;
        PermissionId = permissionId;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid RoleId { get; private set; }
    public Guid PermissionId { get; private set; }

    public static RolePermission Create(Guid tenantId, Guid roleId, Guid permissionId)
    {
        return new RolePermission(tenantId, roleId, permissionId);
    }
}
