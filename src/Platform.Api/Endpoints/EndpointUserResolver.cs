using MediatR;
using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Application.Auth;

namespace Platform.Api.Endpoints;

internal static class EndpointUserResolver
{
    internal static async Task<CurrentUserResponse?> ResolveCurrentUserAsync(
        HttpRequest request,
        IMediator mediator,
        ICurrentUserAccessor currentUserAccessor,
        CancellationToken cancellationToken)
    {
        if (currentUserAccessor.CurrentUser is not null)
        {
            return currentUserAccessor.CurrentUser;
        }

        var accessToken = BearerTokenReader.Read(request);

        return accessToken is null
            ? null
            : await mediator.Send(new GetCurrentUserQuery(accessToken), cancellationToken);
    }
}
