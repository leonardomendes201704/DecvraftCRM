namespace Platform.WebInstaller.ViewModels;

public sealed class TenantStepViewModel
{
    public string CompanyName { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? CustomDomain { get; set; }
}
