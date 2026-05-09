namespace Platform.Application.Abstractions;

public interface IPasswordHashVerifier
{
    bool Verify(string password, string passwordHash);
}
