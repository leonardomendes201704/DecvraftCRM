namespace Platform.Web.Security;

public sealed class PermissionViewPolicy
{
    private readonly IWebUserContext _webUserContext;

    public PermissionViewPolicy(IWebUserContext webUserContext)
    {
        _webUserContext = webUserContext;
    }

    public bool CanView(string permission)
    {
        return _webUserContext.Permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
    }
}
