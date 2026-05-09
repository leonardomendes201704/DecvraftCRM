using System.Security.Claims;
using Platform.Application.Auth;

namespace Platform.Application.Abstractions;

public interface IJwtTokenService
{
    Task<LoginResponse> CreateLoginResponseAsync(CurrentUserResponse user, CancellationToken cancellationToken = default);

    Task<ClaimsPrincipal?> ValidateAsync(string accessToken, CancellationToken cancellationToken = default);
}
