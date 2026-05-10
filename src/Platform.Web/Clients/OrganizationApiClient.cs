namespace Platform.Web.Clients;

public sealed class OrganizationApiClient : PlatformApiClient
{
    public OrganizationApiClient(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }
}
