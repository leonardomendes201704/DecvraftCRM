using Platform.Application.Abstractions;
using Platform.Application.Auth;
using Platform.Domain.Catalog;
using Platform.Domain.Enums;
using Platform.Infrastructure.Auth;

namespace Platform.UnitTests.Authentication;

public sealed class PermissionAuthorizationServiceTests
{
    private const string ValidToken = "valid-token";
    private const string InvalidToken = "invalid-token";

    [Fact]
    public async Task AuthorizeAsync_returns_missing_token_when_access_token_is_empty()
    {
        var service = new PermissionAuthorizationService(new FakeAuthenticationService());

        var result = await service.AuthorizeAsync(null, KnownPermissions.CoreSystemView);

        Assert.Equal(PermissionAuthorizationStatus.MissingToken, result.Status);
    }

    [Fact]
    public async Task AuthorizeAsync_returns_invalid_token_when_user_cannot_be_resolved()
    {
        var service = new PermissionAuthorizationService(new FakeAuthenticationService());

        var result = await service.AuthorizeAsync(InvalidToken, KnownPermissions.CoreSystemView);

        Assert.Equal(PermissionAuthorizationStatus.InvalidToken, result.Status);
    }

    [Fact]
    public async Task AuthorizeAsync_returns_forbidden_when_user_lacks_permission()
    {
        var service = new PermissionAuthorizationService(new FakeAuthenticationService());

        var result = await service.AuthorizeAsync(ValidToken, KnownPermissions.CoreSystemManage);

        Assert.Equal(PermissionAuthorizationStatus.Forbidden, result.Status);
    }

    [Fact]
    public async Task AuthorizeAsync_returns_authorized_when_user_has_permission()
    {
        var service = new PermissionAuthorizationService(new FakeAuthenticationService());

        var result = await service.AuthorizeAsync(ValidToken, KnownPermissions.CoreSystemView);

        Assert.True(result.IsAuthorized);
        Assert.Equal(PermissionAuthorizationStatus.Authorized, result.Status);
    }

    private sealed class FakeAuthenticationService : IAuthenticationService
    {
        public Task<AuthenticationResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task<CurrentUserResponse?> GetCurrentUserAsync(string accessToken, CancellationToken cancellationToken = default)
        {
            if (accessToken != ValidToken)
            {
                return Task.FromResult<CurrentUserResponse?>(null);
            }

            CurrentUserResponse user = new(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "acme",
                "Acme Admin",
                "admin@acme.test",
                [KnownRoles.TenantAdmin],
                [KnownPermissions.CoreSystemView]);

            return Task.FromResult<CurrentUserResponse?>(user);
        }
    }
}
