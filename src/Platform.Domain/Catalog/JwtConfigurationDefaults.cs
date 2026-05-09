namespace Platform.Domain.Catalog;

public static class JwtConfigurationDefaults
{
    public const string Issuer = "WhiteLabelErpCrm";
    public const string Audience = "WhiteLabelErpCrmUsers";
    public const int AccessTokenMinutes = 60;
    public const int SecretSizeInBytes = 64;
}
