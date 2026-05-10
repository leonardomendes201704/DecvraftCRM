using Platform.Api.Routing;
using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Application.Auth;

namespace Platform.Api.Endpoints;

public sealed class AuthEndpointModule : IEndpointModule
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.AuthLogin, async (
            LoginRequest request,
            IAuthenticationService authenticationService,
            CancellationToken cancellationToken) =>
        {
            var result = await authenticationService.LoginAsync(request, cancellationToken);

            return result.Succeeded
                ? Results.Ok(result.Login)
                : Results.Unauthorized();
        })
        .WithName(ApiEndpointNames.Login)
        .WithOpenApi();

        app.MapGet(ApiRoutes.Me, async (
            HttpRequest request,
            IAuthenticationService authenticationService,
            CancellationToken cancellationToken) =>
        {
            var accessToken = BearerTokenReader.Read(request);
            if (accessToken is null)
            {
                return Results.Unauthorized();
            }

            var currentUser = await authenticationService.GetCurrentUserAsync(accessToken, cancellationToken);

            return currentUser is not null
                ? Results.Ok(currentUser)
                : Results.Unauthorized();
        })
        .WithName(ApiEndpointNames.Me)
        .WithOpenApi();
    }
}
