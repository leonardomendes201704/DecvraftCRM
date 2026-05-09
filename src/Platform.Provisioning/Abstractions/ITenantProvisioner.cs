using Platform.Provisioning.Models;

namespace Platform.Provisioning.Abstractions;

public interface ITenantProvisioner
{
    Task<Guid> CreateTenantAsync(TenantSetupOptions options, CancellationToken cancellationToken = default);
    Task<Guid> CreateBrandingAsync(Guid tenantId, BrandingSetupOptions options, CancellationToken cancellationToken = default);
    Task<Guid> CreateAdminUserAsync(Guid tenantId, AdminUserSetupOptions options, CancellationToken cancellationToken = default);
}
