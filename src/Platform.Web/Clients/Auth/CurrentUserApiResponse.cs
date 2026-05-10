namespace Platform.Web.Clients.Auth;

public sealed record CurrentUserApiResponse(
    Guid UserId,
    Guid TenantId,
    string TenantSlug,
    string Name,
    string Email,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions,
    CurrentEmployeeApiResponse? Employee = null);
