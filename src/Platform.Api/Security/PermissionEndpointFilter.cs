using Platform.Application.Abstractions;
using Platform.Domain.Enums;

namespace Platform.Api.Security;

public sealed class PermissionEndpointFilter : IEndpointFilter
{
    private readonly IPermissionAuthorizationService _authorizationService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public PermissionEndpointFilter(
        IPermissionAuthorizationService authorizationService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _authorizationService = authorizationService;
        _currentUserAccessor = currentUserAccessor;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var requiredPermission = context.HttpContext.GetEndpoint()
            ?.Metadata
            .GetMetadata<RequiredPermissionMetadata>()
            ?.Permission;

        if (requiredPermission is null)
        {
            return await next(context);
        }

        var accessToken = BearerTokenReader.Read(context.HttpContext.Request);
        var result = await _authorizationService.AuthorizeAsync(
            accessToken,
            requiredPermission,
            context.HttpContext.RequestAborted);

        if (result.Status == PermissionAuthorizationStatus.Authorized && result.CurrentUser is not null)
        {
            _currentUserAccessor.Set(result.CurrentUser);
        }

        return result.Status switch
        {
            PermissionAuthorizationStatus.Authorized => await next(context),
            PermissionAuthorizationStatus.Forbidden => Results.Forbid(),
            _ => Results.Unauthorized()
        };
    }
}
