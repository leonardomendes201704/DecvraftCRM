using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Platform.Web.Clients;
using Platform.Web.Clients.Auth;
using Platform.Web.Constants;
using Platform.Web.Services.Auth;
using Platform.Web.ViewModels.Account;

namespace Platform.Web.Services;

public sealed class AuthWebService
{
    public AuthWebService(AuthApiClient authApiClient)
    {
        AuthApiClient = authApiClient;
    }

    private AuthApiClient AuthApiClient { get; }

    public async Task<LoginOperationResult> LoginAsync(
        LoginViewModel input,
        CancellationToken cancellationToken)
    {
        if (!TryBuildApiBaseUrl(input.ApiBaseUrl, out var apiBaseUrl))
        {
            return LoginOperationResult.Failure(AuthErrorMessages.InvalidApiBaseUrl);
        }

        try
        {
            var login = await AuthApiClient.LoginAsync(
                apiBaseUrl,
                new LoginApiRequest(input.TenantSlug, input.Email, input.Password),
                cancellationToken);

            if (login is null)
            {
                return LoginOperationResult.Failure(AuthErrorMessages.InvalidCredentials);
            }

            var currentUser = await AuthApiClient.GetCurrentUserAsync(
                apiBaseUrl,
                login.AccessToken,
                cancellationToken);

            return currentUser is null
                ? LoginOperationResult.Failure(AuthErrorMessages.CurrentUserUnavailable)
                : LoginOperationResult.Success(login, currentUser);
        }
        catch (HttpRequestException)
        {
            return LoginOperationResult.Failure(AuthErrorMessages.ApiUnavailable);
        }
        catch (TaskCanceledException)
        {
            return LoginOperationResult.Failure(AuthErrorMessages.ApiUnavailable);
        }
    }

    public ClaimsPrincipal BuildPrincipal(
        LoginApiResponse login,
        CurrentUserApiResponse currentUser,
        Uri apiBaseUrl)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, currentUser.UserId.ToString()),
            new(ClaimTypes.Name, currentUser.Name),
            new(ClaimTypes.Email, currentUser.Email),
            new(WebAuthenticationDefaults.AccessTokenClaim, login.AccessToken),
            new(WebAuthenticationDefaults.ExpiresAtClaim, login.ExpiresAt.ToString("O")),
            new(WebAuthenticationDefaults.ApiBaseUrlClaim, apiBaseUrl.ToString()),
            new(WebAuthenticationDefaults.TenantIdClaim, currentUser.TenantId.ToString()),
            new(WebAuthenticationDefaults.TenantSlugClaim, currentUser.TenantSlug)
        };

        claims.AddRange(currentUser.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
        claims.AddRange(currentUser.Permissions.Select(permission => new Claim(WebAuthenticationDefaults.PermissionClaim, permission)));

        if (currentUser.Employee is not null)
        {
            claims.Add(new Claim(WebAuthenticationDefaults.EmployeeIdClaim, currentUser.Employee.Id.ToString()));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }

    public AuthenticationProperties BuildAuthenticationProperties(DateTimeOffset expiresAt)
    {
        return new AuthenticationProperties
        {
            AllowRefresh = false,
            ExpiresUtc = expiresAt,
            IsPersistent = false,
            IssuedUtc = DateTimeOffset.UtcNow
        };
    }

    public bool TryBuildApiBaseUrl(string value, out Uri apiBaseUrl)
    {
        apiBaseUrl = null!;

        if (!Uri.TryCreate(value?.Trim(), UriKind.Absolute, out var parsed) ||
            (parsed.Scheme != Uri.UriSchemeHttp && parsed.Scheme != Uri.UriSchemeHttps))
        {
            return false;
        }

        apiBaseUrl = parsed;
        return true;
    }
}
