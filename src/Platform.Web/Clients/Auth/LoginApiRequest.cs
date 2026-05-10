namespace Platform.Web.Clients.Auth;

public sealed record LoginApiRequest(string TenantSlug, string Email, string Password);
