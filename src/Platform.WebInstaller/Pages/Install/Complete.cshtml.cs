using Platform.WebInstaller.Services;
using Platform.WebInstaller.Wizard;

namespace Platform.WebInstaller.Pages.Install;

public sealed class CompleteModel : InstallPageModel
{
    public CompleteModel(IInstallerWizardStateStore stateStore)
        : base(stateStore)
    {
    }

    public override InstallerWizardStep CurrentStep => InstallerWizardStep.Complete;

    public void OnGet()
    {
        LoadWizardState();
    }
}
