namespace Platform.Provisioning.Models;

public sealed class InstallRequest
{
    public DatabaseSetupOptions Database { get; set; } = new();
    public TenantSetupOptions Tenant { get; set; } = new();
    public AdminUserSetupOptions AdminUser { get; set; } = new();
    public BrandingSetupOptions Branding { get; set; } = new();
    public List<string> Modules { get; set; } = [];
}
