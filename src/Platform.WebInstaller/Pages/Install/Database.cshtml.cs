using Microsoft.AspNetCore.Mvc;
using Platform.WebInstaller.Services;
using Platform.WebInstaller.ViewModels;
using Platform.WebInstaller.Wizard;

namespace Platform.WebInstaller.Pages.Install;

public sealed class DatabaseModel : InstallPageModel
{
    public DatabaseModel(IInstallerWizardStateStore stateStore)
        : base(stateStore)
    {
    }

    public override InstallerWizardStep CurrentStep => InstallerWizardStep.Database;

    [BindProperty]
    public DatabaseStepViewModel Input { get; set; } = new();

    public void OnGet()
    {
        Input = LoadWizardState().Database;
    }

    public IActionResult OnPost()
    {
        var state = LoadWizardState();
        state.Database = Input;
        SaveWizardState(state);

        return Redirect(InstallerWizardRoutes.Tenant);
    }
}
