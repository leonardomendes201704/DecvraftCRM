namespace Platform.Web.Clients;

public sealed class CrmApiClient : PlatformApiClient
{
    public CrmApiClient(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }
}
