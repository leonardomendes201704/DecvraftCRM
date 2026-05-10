namespace Platform.WebInstaller.ViewModels;

public sealed class DatabaseStepViewModel
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 1433;
    public string DatabaseName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool TrustServerCertificate { get; set; } = true;
    public bool HasConnectionTest { get; set; }
    public bool LastConnectionTestSucceeded { get; set; }
}
