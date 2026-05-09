namespace Platform.Api.Security;

public static class AuthorizationEndpointExtensions
{
    public static RouteHandlerBuilder RequirePermission(this RouteHandlerBuilder builder, string permission)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(permission);

        return builder
            .WithMetadata(new RequiredPermissionMetadata(permission))
            .AddEndpointFilter<PermissionEndpointFilter>();
    }
}
