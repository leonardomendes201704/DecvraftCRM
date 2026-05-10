using System.Text.Json;
using Platform.WebInstaller.ViewModels;

namespace Platform.WebInstaller.Services;

public sealed class SessionInstallerWizardStateStore : IInstallerWizardStateStore
{
    private const string SessionKey = "InstallerWizardState";

    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public InstallWizardState Get(HttpContext httpContext)
    {
        var serializedState = httpContext.Session.GetString(SessionKey);

        if (string.IsNullOrWhiteSpace(serializedState))
        {
            return new InstallWizardState();
        }

        return JsonSerializer.Deserialize<InstallWizardState>(serializedState, SerializerOptions)
            ?? new InstallWizardState();
    }

    public void Save(HttpContext httpContext, InstallWizardState state)
    {
        var serializedState = JsonSerializer.Serialize(state, SerializerOptions);
        httpContext.Session.SetString(SessionKey, serializedState);
    }

    public void Clear(HttpContext httpContext)
    {
        httpContext.Session.Remove(SessionKey);
    }
}
