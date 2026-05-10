namespace Platform.WebInstaller.ViewModels;

public sealed class InstallWizardState
{
    public DatabaseStepViewModel Database { get; set; } = new();
    public TenantStepViewModel Tenant { get; set; } = new();
    public AdminStepViewModel Admin { get; set; } = new();
    public BrandingStepViewModel Branding { get; set; } = new();
    public ModulesStepViewModel Modules { get; set; } = new();
}
