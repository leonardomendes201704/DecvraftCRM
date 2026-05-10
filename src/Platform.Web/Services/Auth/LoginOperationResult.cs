using Platform.Web.Clients.Auth;

namespace Platform.Web.Services.Auth;

public sealed record LoginOperationResult(
    bool Succeeded,
    LoginApiResponse? Login,
    CurrentUserApiResponse? CurrentUser,
    string? ErrorMessage)
{
    public static LoginOperationResult Success(LoginApiResponse login, CurrentUserApiResponse currentUser)
    {
        return new LoginOperationResult(true, login, currentUser, null);
    }

    public static LoginOperationResult Failure(string errorMessage)
    {
        return new LoginOperationResult(false, null, null, errorMessage);
    }
}
