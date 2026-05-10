namespace Platform.Web.Constants;

public static class WebAuthenticationDefaults
{
    public const string CookieName = "DevcraftCRM.Auth";
    public const string AccessTokenClaim = "access_token";
    public const string ExpiresAtClaim = "expires_at";
    public const string ApiBaseUrlClaim = "api_base_url";
    public const string TenantIdClaim = "tenant_id";
    public const string TenantSlugClaim = "tenant_slug";
    public const string PermissionClaim = "permission";
    public const string EmployeeIdClaim = "employee_id";
}
