using Microsoft.AspNetCore.Mvc;
using Platform.Provisioning.Abstractions;
using Platform.WebInstaller.Services;
using Platform.WebInstaller.ViewModels;
using Platform.WebInstaller.Wizard;

namespace Platform.WebInstaller.Pages.Install;

public sealed class ReviewModel : InstallPageModel
{
    private readonly IProvisioningService _provisioningService;

    public ReviewModel(
        IInstallerWizardStateStore stateStore,
        IProvisioningService provisioningService)
        : base(stateStore)
    {
        _provisioningService = provisioningService;
    }

    public override InstallerWizardStep CurrentStep => InstallerWizardStep.Review;

    public void OnGet()
    {
        LoadWizardState();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var state = LoadWizardState();
        PageErrors = InstallerWizardValidator.ValidateReadyToInstall(state);

        if (PageErrors.Count > 0)
        {
            return Page();
        }

        var result = await _provisioningService.InstallAsync(
            InstallerWizardRequestFactory.ToInstallRequest(state),
            cancellationToken);

        if (!result.Succeeded)
        {
            PageErrors = result.Errors;
            return Page();
        }

        SaveWizardState(new InstallWizardState
        {
            Completion = new InstallCompletionViewModel
            {
                Succeeded = true,
                TenantId = result.TenantId,
                AdminUserId = result.AdminUserId,
                InstalledModules = result.InstalledModules.ToList()
            }
        });

        return Redirect(InstallerWizardRoutes.Complete);
    }
}
