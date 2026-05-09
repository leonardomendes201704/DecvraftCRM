namespace Platform.Provisioning.Models;

public sealed class TenantSetupOptions
{
    public string CompanyName { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? CustomDomain { get; set; }
}
