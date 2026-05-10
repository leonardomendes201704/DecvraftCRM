using Platform.WebInstaller.Services;
using Platform.WebInstaller.Wizard;

namespace Platform.WebInstaller.Pages.Install;

public sealed class ReviewModel : InstallPageModel
{
    public ReviewModel(IInstallerWizardStateStore stateStore)
        : base(stateStore)
    {
    }

    public override InstallerWizardStep CurrentStep => InstallerWizardStep.Review;

    public void OnGet()
    {
        LoadWizardState();
    }
}
