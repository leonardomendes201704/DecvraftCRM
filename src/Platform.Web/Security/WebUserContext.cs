using System.Security.Claims;
using Platform.Web.Constants;

namespace Platform.Web.Security;

public sealed class WebUserContext : IWebUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WebUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId => TryReadGuid(ClaimTypes.NameIdentifier);

    public Guid? TenantId => TryReadGuid(WebAuthenticationDefaults.TenantIdClaim);

    public string TenantSlug => ReadClaim(WebAuthenticationDefaults.TenantSlugClaim);

    public string DisplayName => ReadClaim(ClaimTypes.Name, "Usuario nao autenticado");

    public string TenantName => string.IsNullOrWhiteSpace(TenantSlug) ? "Tenant" : TenantSlug;

    public string Email => ReadClaim(ClaimTypes.Email);

    public string? AccessToken => ReadClaim(WebAuthenticationDefaults.AccessTokenClaim);

    public Uri? ApiBaseUrl =>
        Uri.TryCreate(ReadClaim(WebAuthenticationDefaults.ApiBaseUrlClaim), UriKind.Absolute, out var apiBaseUrl)
            ? apiBaseUrl
            : null;

    public IReadOnlyCollection<string> Permissions =>
        User.Claims
            .Where(claim => claim.Type == WebAuthenticationDefaults.PermissionClaim)
            .Select(claim => claim.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();

    private string ReadClaim(string claimType, string fallback = "")
    {
        return User.FindFirst(claimType)?.Value ?? fallback;
    }

    private Guid? TryReadGuid(string claimType)
    {
        return Guid.TryParse(ReadClaim(claimType), out var value) ? value : null;
    }
}
