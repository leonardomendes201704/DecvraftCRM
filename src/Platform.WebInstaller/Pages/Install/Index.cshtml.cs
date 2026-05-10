using Platform.Provisioning.Abstractions;
using Platform.WebInstaller.Services;
using Platform.WebInstaller.Wizard;

namespace Platform.WebInstaller.Pages.Install;

public sealed class IndexModel : InstallPageModel
{
    private readonly IInstallerLockService _installerLockService;

    public IndexModel(
        IInstallerWizardStateStore stateStore,
        IInstallerLockService installerLockService)
        : base(stateStore)
    {
        _installerLockService = installerLockService;
    }

    public override InstallerWizardStep CurrentStep => InstallerWizardStep.Welcome;

    public bool IsInstalled { get; private set; }

    public string? Version { get; private set; }

    public DateTimeOffset? InstalledAt { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        LoadWizardState();

        try
        {
            var status = await _installerLockService.GetStatusAsync(cancellationToken);
            IsInstalled = status.IsInstalled;
            Version = status.Version;
            InstalledAt = status.InstalledAt;
        }
        catch
        {
            IsInstalled = false;
        }
    }
}
