using Platform.Application.Abstractions;
using Platform.Application.Authorization;

namespace Platform.Infrastructure.Auth;

public sealed class PermissionAuthorizationService : IPermissionAuthorizationService
{
    private readonly IAuthenticationService _authenticationService;

    public PermissionAuthorizationService(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<PermissionAuthorizationResult> AuthorizeAsync(
        string? accessToken,
        string requiredPermission,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(requiredPermission);

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return PermissionAuthorizationResult.MissingToken(requiredPermission);
        }

        var currentUser = await _authenticationService.GetCurrentUserAsync(accessToken, cancellationToken);
        if (currentUser is null)
        {
            return PermissionAuthorizationResult.InvalidToken(requiredPermission);
        }

        return currentUser.Permissions.Contains(requiredPermission, StringComparer.Ordinal)
            ? PermissionAuthorizationResult.Authorized(requiredPermission)
            : PermissionAuthorizationResult.Forbidden(requiredPermission);
    }
}
