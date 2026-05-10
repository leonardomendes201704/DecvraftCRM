namespace Platform.Application.Auth;

public sealed record CurrentUserResponse(
    Guid UserId,
    Guid TenantId,
    string TenantSlug,
    string Name,
    string Email,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions,
    CurrentEmployeeResponse? Employee = null);
