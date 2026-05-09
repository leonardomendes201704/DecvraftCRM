using System.Globalization;
using System.Security.Cryptography;
using Platform.Domain.Security;
using Platform.Infrastructure.Auth;

namespace Platform.UnitTests.Authentication;

public sealed class PasswordHashVerifierTests
{
    [Fact]
    public void Verify_returns_true_for_valid_pbkdf2_hash()
    {
        var hash = CreatePasswordHash("StrongPassword123!");
        var verifier = new PasswordHashVerifier();

        var result = verifier.Verify("StrongPassword123!", hash);

        Assert.True(result);
    }

    [Fact]
    public void Verify_returns_false_for_invalid_password()
    {
        var hash = CreatePasswordHash("StrongPassword123!");
        var verifier = new PasswordHashVerifier();

        var result = verifier.Verify("WrongPassword123!", hash);

        Assert.False(result);
    }

    private static string CreatePasswordHash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(PasswordHashingDefaults.SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            PasswordHashingDefaults.Iterations,
            HashAlgorithmName.SHA256,
            PasswordHashingDefaults.HashSize);

        return string.Join(
            "$",
            PasswordHashingDefaults.Algorithm,
            PasswordHashingDefaults.Iterations.ToString(CultureInfo.InvariantCulture),
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }
}
