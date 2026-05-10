using MediatR;

namespace Platform.Application.Auth;

public sealed record GetCurrentUserQuery(string AccessToken) : IRequest<CurrentUserResponse?>;
