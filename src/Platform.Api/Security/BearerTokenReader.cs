namespace Platform.Api.Security;

public static class BearerTokenReader
{
    public static string? Read(HttpRequest request)
    {
        var authorization = request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authorization) ||
            !authorization.StartsWith(ApiAuthenticationDefaults.BearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return authorization[ApiAuthenticationDefaults.BearerPrefix.Length..].Trim();
    }
}
