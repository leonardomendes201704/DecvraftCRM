using Platform.Application.Abstractions;
using Platform.Domain.Enums;

namespace Platform.Api.Security;

public sealed class PermissionEndpointFilter : IEndpointFilter
{
    private readonly IPermissionAuthorizationService _authorizationService;

    public PermissionEndpointFilter(IPermissionAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
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

        return result.Status switch
        {
            PermissionAuthorizationStatus.Authorized => await next(context),
            PermissionAuthorizationStatus.Forbidden => Results.Forbid(),
            _ => Results.Unauthorized()
        };
    }
}
