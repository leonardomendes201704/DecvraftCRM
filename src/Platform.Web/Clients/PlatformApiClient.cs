namespace Platform.Web.Clients;

public class PlatformApiClient
{
    protected readonly IHttpClientFactory HttpClientFactory;

    public PlatformApiClient(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
    }
}
