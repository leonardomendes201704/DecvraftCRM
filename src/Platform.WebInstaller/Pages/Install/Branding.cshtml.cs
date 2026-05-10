using Microsoft.AspNetCore.Mvc;
using Platform.WebInstaller.Services;
using Platform.WebInstaller.ViewModels;
using Platform.WebInstaller.Wizard;

namespace Platform.WebInstaller.Pages.Install;

public sealed class BrandingModel : InstallPageModel
{
    public BrandingModel(IInstallerWizardStateStore stateStore)
        : base(stateStore)
    {
    }

    public override InstallerWizardStep CurrentStep => InstallerWizardStep.Branding;

    [BindProperty]
    public BrandingStepViewModel Input { get; set; } = new();

    public void OnGet()
    {
        Input = LoadWizardState().Branding;
    }

    public IActionResult OnPost()
    {
        var state = LoadWizardState();
        state.Branding = Input;
        SaveWizardState(state);

        return Redirect(InstallerWizardRoutes.Modules);
    }
}
