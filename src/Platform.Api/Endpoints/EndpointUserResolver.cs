using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Application.Auth;

namespace Platform.Api.Endpoints;

internal static class EndpointUserResolver
{
    internal static async Task<CurrentUserResponse?> ResolveCurrentUserAsync(
        HttpRequest request,
        IAuthenticationService authenticationService,
        CancellationToken cancellationToken)
    {
        var accessToken = BearerTokenReader.Read(request);

        return accessToken is null
            ? null
            : await authenticationService.GetCurrentUserAsync(accessToken, cancellationToken);
    }
}
