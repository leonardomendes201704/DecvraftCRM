using Platform.Api.Routing;
using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Domain.Catalog;

namespace Platform.Api.Endpoints;

public sealed class ModuleEndpointModule : IEndpointModule
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Modules, async (
            HttpRequest request,
            IAuthenticationService authenticationService,
            IModuleCatalogService moduleCatalogService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(
                request,
                authenticationService,
                cancellationToken);

            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var modules = await moduleCatalogService.GetModulesAsync(currentUser.TenantId, cancellationToken);

            return Results.Ok(modules);
        })
        .RequirePermission(KnownPermissions.CoreModulesView)
        .WithName(ApiEndpointNames.Modules)
        .WithOpenApi();

        app.MapGet(ApiRoutes.SystemPermissions, () => KnownPermissions.All)
            .RequirePermission(KnownPermissions.CoreSystemView)
            .WithName(ApiEndpointNames.SystemPermissions)
            .WithOpenApi();
    }
}
