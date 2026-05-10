using MediatR;
using Platform.Api.Routing;
using Platform.Api.Security;
using Platform.Application.Auth;
using Platform.Application.Modules;
using Platform.Domain.Catalog;

namespace Platform.Api.Endpoints;

public sealed class ModuleEndpointModule : IEndpointModule
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Modules, async (
            HttpRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var accessToken = BearerTokenReader.Read(request);
            var currentUser = accessToken is null
                ? null
                : await mediator.Send(new GetCurrentUserQuery(accessToken), cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var modules = await mediator.Send(new ListModulesQuery(currentUser.TenantId), cancellationToken);

            return Results.Ok(modules);
        })
        .RequirePermission(KnownPermissions.CoreModulesView)
        .WithName(ApiEndpointNames.Modules)
        .WithOpenApi();

        app.MapGet(ApiRoutes.SystemPermissions, async (IMediator mediator, CancellationToken cancellationToken) =>
                await mediator.Send(new ListSystemPermissionsQuery(), cancellationToken))
            .RequirePermission(KnownPermissions.CoreSystemView)
            .WithName(ApiEndpointNames.SystemPermissions)
            .WithOpenApi();
    }
}
