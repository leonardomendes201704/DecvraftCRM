using Platform.Domain.Enums;

namespace Platform.Application.Authorization;

public sealed record PermissionAuthorizationResult(
    PermissionAuthorizationStatus Status,
    string? Permission)
{
    public bool IsAuthorized => Status == PermissionAuthorizationStatus.Authorized;

    public static PermissionAuthorizationResult Authorized(string permission) =>
        new(PermissionAuthorizationStatus.Authorized, permission);

    public static PermissionAuthorizationResult MissingToken(string permission) =>
        new(PermissionAuthorizationStatus.MissingToken, permission);

    public static PermissionAuthorizationResult InvalidToken(string permission) =>
        new(PermissionAuthorizationStatus.InvalidToken, permission);

    public static PermissionAuthorizationResult Forbidden(string permission) =>
        new(PermissionAuthorizationStatus.Forbidden, permission);
}
