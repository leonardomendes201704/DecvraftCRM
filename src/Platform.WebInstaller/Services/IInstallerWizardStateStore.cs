using Platform.WebInstaller.ViewModels;

namespace Platform.WebInstaller.Services;

public interface IInstallerWizardStateStore
{
    InstallWizardState Get(HttpContext httpContext);

    void Save(HttpContext httpContext, InstallWizardState state);

    void Clear(HttpContext httpContext);
}
