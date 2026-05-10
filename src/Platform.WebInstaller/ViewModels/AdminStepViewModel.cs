namespace Platform.WebInstaller.ViewModels;

public sealed class AdminStepViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public bool HasPassword => !string.IsNullOrWhiteSpace(Password);
}
