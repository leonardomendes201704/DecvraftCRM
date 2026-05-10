using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Auth;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return _authenticationService.LoginAsync(request.Request, cancellationToken);
    }
}
