namespace Platform.WebInstaller.ViewModels;

public sealed class InstallCompletionViewModel
{
    public bool Succeeded { get; set; }
    public Guid? TenantId { get; set; }
    public Guid? AdminUserId { get; set; }
    public List<string> InstalledModules { get; set; } = [];
    public List<string> Errors { get; set; } = [];
}
