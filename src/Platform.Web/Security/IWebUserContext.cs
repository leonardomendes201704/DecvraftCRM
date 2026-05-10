namespace Platform.Web.Security;

public interface IWebUserContext
{
    Guid? UserId { get; }
    Guid? TenantId { get; }
    string TenantSlug { get; }
    string DisplayName { get; }
    string TenantName { get; }
    string Email { get; }
    string? AccessToken { get; }
    Uri? ApiBaseUrl { get; }
    IReadOnlyCollection<string> Permissions { get; }
}
