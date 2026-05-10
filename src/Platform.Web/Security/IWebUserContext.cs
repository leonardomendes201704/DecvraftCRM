namespace Platform.Web.Security;

public interface IWebUserContext
{
    string DisplayName { get; }
    string TenantName { get; }
    IReadOnlyCollection<string> Permissions { get; }
}
