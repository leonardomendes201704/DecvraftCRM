namespace Platform.Web.Clients;

public sealed class AuthApiClient : PlatformApiClient
{
    public AuthApiClient(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }
}
