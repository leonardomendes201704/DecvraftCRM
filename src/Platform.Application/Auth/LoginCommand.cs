using MediatR;

namespace Platform.Application.Auth;

public sealed record LoginCommand(LoginRequest Request) : IRequest<AuthenticationResult>;
