using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Auth;

public sealed class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse?>
{
    private readonly IAuthenticationService _authenticationService;

    public GetCurrentUserQueryHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public Task<CurrentUserResponse?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        return _authenticationService.GetCurrentUserAsync(request.AccessToken, cancellationToken);
    }
}
