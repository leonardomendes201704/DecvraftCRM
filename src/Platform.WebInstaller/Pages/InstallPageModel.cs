using Microsoft.AspNetCore.Mvc.RazorPages;
using Platform.WebInstaller.Services;
using Platform.WebInstaller.ViewModels;
using Platform.WebInstaller.Wizard;

namespace Platform.WebInstaller.Pages;

public abstract class InstallPageModel : PageModel
{
    private readonly IInstallerWizardStateStore _stateStore;

    protected InstallPageModel(IInstallerWizardStateStore stateStore)
    {
        _stateStore = stateStore;
    }

    public abstract InstallerWizardStep CurrentStep { get; }

    public IReadOnlyCollection<InstallerWizardStepDefinition> Steps => InstallerWizardSteps.All;

    public InstallWizardState WizardState { get; private set; } = new();

    public IReadOnlyCollection<string> PageErrors { get; protected set; } = [];

    public string? PageSuccessMessage { get; protected set; }

    protected InstallWizardState LoadWizardState()
    {
        WizardState = _stateStore.Get(HttpContext);
        return WizardState;
    }

    protected void SaveWizardState(InstallWizardState state)
    {
        _stateStore.Save(HttpContext, state);
        WizardState = state;
    }

    protected void ClearWizardState()
    {
        _stateStore.Clear(HttpContext);
        WizardState = new InstallWizardState();
    }
}
