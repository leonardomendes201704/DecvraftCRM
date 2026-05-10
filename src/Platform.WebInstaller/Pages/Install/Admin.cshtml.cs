using Microsoft.AspNetCore.Mvc;
using Platform.WebInstaller.Services;
using Platform.WebInstaller.ViewModels;
using Platform.WebInstaller.Wizard;

namespace Platform.WebInstaller.Pages.Install;

public sealed class AdminModel : InstallPageModel
{
    public AdminModel(IInstallerWizardStateStore stateStore)
        : base(stateStore)
    {
    }

    public override InstallerWizardStep CurrentStep => InstallerWizardStep.Admin;

    [BindProperty]
    public AdminStepViewModel Input { get; set; } = new();

    [BindProperty]
    public string ConfirmPassword { get; set; } = string.Empty;

    public void OnGet()
    {
        Input = LoadWizardState().Admin;
    }

    public IActionResult OnPost()
    {
        var state = LoadWizardState();
        PageErrors = InstallerWizardValidator.ValidateAdmin(Input, ConfirmPassword);

        if (PageErrors.Count > 0)
        {
            Input.Password = string.Empty;
            return Page();
        }

        state.Admin = Input;
        SaveWizardState(state);

        return Redirect(InstallerWizardRoutes.Branding);
    }
}
