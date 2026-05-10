using MediatR;
using Platform.Api.Routing;
using Platform.Api.Security;
using Platform.Application.Auth;

namespace Platform.Api.Endpoints;

public sealed class AuthEndpointModule : IEndpointModule
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.AuthLogin, async (
            LoginRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new LoginCommand(request), cancellationToken);

            return result.Succeeded
                ? Results.Ok(result.Login)
                : Results.Unauthorized();
        })
        .WithName(ApiEndpointNames.Login)
        .WithOpenApi();

        app.MapGet(ApiRoutes.Me, async (
            HttpRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var accessToken = BearerTokenReader.Read(request);
            if (accessToken is null)
            {
                return Results.Unauthorized();
            }

            var currentUser = await mediator.Send(new GetCurrentUserQuery(accessToken), cancellationToken);

            return currentUser is not null
                ? Results.Ok(currentUser)
                : Results.Unauthorized();
        })
        .WithName(ApiEndpointNames.Me)
        .WithOpenApi();
    }
}
