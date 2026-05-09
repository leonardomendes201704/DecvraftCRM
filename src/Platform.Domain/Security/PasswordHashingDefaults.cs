namespace Platform.Domain.Security;

public static class PasswordHashingDefaults
{
    public const int SaltSize = 16;
    public const int HashSize = 32;
    public const int Iterations = 100_000;
    public const string Algorithm = "PBKDF2-SHA256";
}
