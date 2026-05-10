namespace Platform.Web.Security;

public sealed class WebUserContext : IWebUserContext
{
    public string DisplayName => "Usuario nao autenticado";

    public string TenantName => "Tenant";

    public IReadOnlyCollection<string> Permissions => [];
}
