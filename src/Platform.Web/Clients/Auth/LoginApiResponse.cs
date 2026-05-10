namespace Platform.Web.Clients.Auth;

public sealed record LoginApiResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    CurrentUserApiResponse User);
