using Platform.Application.Auth;
using Platform.Domain.Enums;

namespace Platform.Application.Authorization;

public sealed record PermissionAuthorizationResult(
    PermissionAuthorizationStatus Status,
    string? Permission,
    CurrentUserResponse? CurrentUser)
{
    public bool IsAuthorized => Status == PermissionAuthorizationStatus.Authorized;

    public static PermissionAuthorizationResult Authorized(string permission, CurrentUserResponse currentUser) =>
        new(PermissionAuthorizationStatus.Authorized, permission, currentUser);

    public static PermissionAuthorizationResult MissingToken(string permission) =>
        new(PermissionAuthorizationStatus.MissingToken, permission, null);

    public static PermissionAuthorizationResult InvalidToken(string permission) =>
        new(PermissionAuthorizationStatus.InvalidToken, permission, null);

    public static PermissionAuthorizationResult Forbidden(string permission) =>
        new(PermissionAuthorizationStatus.Forbidden, permission, null);
}
