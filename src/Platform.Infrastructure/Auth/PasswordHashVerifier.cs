using System.Globalization;
using System.Security.Cryptography;
using Platform.Application.Abstractions;
using Platform.Domain.Security;

namespace Platform.Infrastructure.Auth;

public sealed class PasswordHashVerifier : IPasswordHashVerifier
{
    private const int ExpectedHashParts = 4;
    private const int AlgorithmIndex = 0;
    private const int IterationsIndex = 1;
    private const int SaltIndex = 2;
    private const int HashIndex = 3;

    public bool Verify(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
        {
            return false;
        }

        var parts = passwordHash.Split('$');
        if (parts.Length != ExpectedHashParts || parts[AlgorithmIndex] != PasswordHashingDefaults.Algorithm)
        {
            return false;
        }

        if (!int.TryParse(parts[IterationsIndex], NumberStyles.Integer, CultureInfo.InvariantCulture, out var iterations))
        {
            return false;
        }

        try
        {
            var salt = Convert.FromBase64String(parts[SaltIndex]);
            var expectedHash = Convert.FromBase64String(parts[HashIndex]);
            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
