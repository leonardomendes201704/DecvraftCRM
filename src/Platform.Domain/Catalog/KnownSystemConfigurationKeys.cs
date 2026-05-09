namespace Platform.Domain.Catalog;

public static class KnownSystemConfigurationKeys
{
    public const string JwtSecret = "security.jwt.secret";
    public const string JwtIssuer = "security.jwt.issuer";
    public const string JwtAudience = "security.jwt.audience";
    public const string JwtAccessTokenMinutes = "security.jwt.access_token_minutes";
}
