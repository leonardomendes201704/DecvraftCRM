using Platform.Provisioning.Models;
using Platform.WebInstaller.ViewModels;

namespace Platform.WebInstaller.Services;

public static class InstallerWizardRequestFactory
{
    public static DatabaseSetupOptions ToDatabaseOptions(DatabaseStepViewModel input)
    {
        return new DatabaseSetupOptions
        {
            Host = input.Host.Trim(),
            Port = input.Port,
            DatabaseName = input.DatabaseName.Trim(),
            Username = input.Username.Trim(),
            Password = input.Password,
            TrustServerCertificate = input.TrustServerCertificate
        };
    }

    public static InstallRequest ToInstallRequest(InstallWizardState state)
    {
        return new InstallRequest
        {
            Database = ToDatabaseOptions(state.Database),
            Tenant = new TenantSetupOptions
            {
                CompanyName = state.Tenant.CompanyName.Trim(),
                Slug = state.Tenant.Slug.Trim(),
                CustomDomain = string.IsNullOrWhiteSpace(state.Tenant.CustomDomain)
                    ? null
                    : state.Tenant.CustomDomain.Trim()
            },
            AdminUser = new AdminUserSetupOptions
            {
                Name = state.Admin.Name.Trim(),
                Email = state.Admin.Email.Trim(),
                Password = state.Admin.Password
            },
            Branding = new BrandingSetupOptions
            {
                SystemName = state.Branding.SystemName.Trim(),
                PrimaryColor = state.Branding.PrimaryColor,
                SecondaryColor = state.Branding.SecondaryColor
            },
            Modules = state.Modules.SelectedModuleSlugs.ToList()
        };
    }
}
