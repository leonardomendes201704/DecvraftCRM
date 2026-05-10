using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Platform.Web.Clients.Auth;
using Platform.Web.Constants;

namespace Platform.Web.Clients;

public sealed class AuthApiClient : PlatformApiClient
{
    public AuthApiClient(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }

    public async Task<LoginApiResponse?> LoginAsync(
        Uri apiBaseUrl,
        LoginApiRequest request,
        CancellationToken cancellationToken)
    {
        var httpClient = CreateClient(apiBaseUrl);
        var response = await httpClient.PostAsJsonAsync("api/auth/login", request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LoginApiResponse>(cancellationToken);
    }

    public async Task<CurrentUserApiResponse?> GetCurrentUserAsync(
        Uri apiBaseUrl,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var httpClient = CreateClient(apiBaseUrl);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            accessToken);

        var response = await httpClient.GetAsync("api/me", cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CurrentUserApiResponse>(cancellationToken);
    }

    private HttpClient CreateClient(Uri apiBaseUrl)
    {
        var httpClient = HttpClientFactory.CreateClient();
        httpClient.BaseAddress = NormalizeBaseUrl(apiBaseUrl);
        return httpClient;
    }

    private static Uri NormalizeBaseUrl(Uri apiBaseUrl)
    {
        var builder = new UriBuilder(apiBaseUrl)
        {
            Path = apiBaseUrl.AbsolutePath.TrimEnd('/') + "/"
        };

        return builder.Uri;
    }
}
