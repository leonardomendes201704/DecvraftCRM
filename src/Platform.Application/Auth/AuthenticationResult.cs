namespace Platform.Application.Auth;

public sealed record AuthenticationResult(
    bool Succeeded,
    LoginResponse? Login,
    string? Error)
{
    public static AuthenticationResult Success(LoginResponse login) => new(true, login, null);

    public static AuthenticationResult Failure(string error) => new(false, null, error);
}
