using Platform.Domain.Common;

namespace Platform.Domain.Entities;

public sealed class UserRole : ITenantEntity
{
    private UserRole()
    {
    }

    private UserRole(Guid tenantId, Guid userId, Guid roleId)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        UserId = userId;
        RoleId = roleId;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid RoleId { get; private set; }

    public static UserRole Create(Guid tenantId, Guid userId, Guid roleId)
    {
        return new UserRole(tenantId, userId, roleId);
    }
}
