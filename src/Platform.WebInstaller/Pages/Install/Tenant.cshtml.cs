using Microsoft.AspNetCore.Mvc;
using Platform.WebInstaller.Services;
using Platform.WebInstaller.ViewModels;
using Platform.WebInstaller.Wizard;

namespace Platform.WebInstaller.Pages.Install;

public sealed class TenantModel : InstallPageModel
{
    public TenantModel(IInstallerWizardStateStore stateStore)
        : base(stateStore)
    {
    }

    public override InstallerWizardStep CurrentStep => InstallerWizardStep.Tenant;

    [BindProperty]
    public TenantStepViewModel Input { get; set; } = new();

    public void OnGet()
    {
        Input = LoadWizardState().Tenant;
    }

    public IActionResult OnPost()
    {
        var state = LoadWizardState();
        state.Tenant = Input;
        SaveWizardState(state);

        return Redirect(InstallerWizardRoutes.Admin);
    }
}
