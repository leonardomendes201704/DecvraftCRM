using Platform.Application.Authorization;

namespace Platform.Application.Abstractions;

public interface IPermissionAuthorizationService
{
    Task<PermissionAuthorizationResult> AuthorizeAsync(
        string? accessToken,
        string requiredPermission,
        CancellationToken cancellationToken = default);
}
