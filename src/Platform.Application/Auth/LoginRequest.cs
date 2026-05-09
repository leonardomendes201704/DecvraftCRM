namespace Platform.Application.Auth;

public sealed record LoginRequest(
    string TenantSlug,
    string Email,
    string Password);
