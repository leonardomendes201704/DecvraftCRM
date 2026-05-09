namespace Platform.Application.Auth;

public sealed record LoginResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    CurrentUserResponse User);
