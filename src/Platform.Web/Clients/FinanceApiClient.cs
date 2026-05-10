namespace Platform.Web.Clients;

public sealed class FinanceApiClient : PlatformApiClient
{
    public FinanceApiClient(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }
}
