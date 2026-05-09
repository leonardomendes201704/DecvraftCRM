using Platform.Application.Auth;

namespace Platform.Application.Abstractions;

public interface IAuthenticationService
{
    Task<AuthenticationResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    Task<CurrentUserResponse?> GetCurrentUserAsync(string accessToken, CancellationToken cancellationToken = default);
}
