using Platform.Web.Clients;

namespace Platform.Web.Services;

public sealed class AuthWebService
{
    public AuthWebService(AuthApiClient authApiClient)
    {
        AuthApiClient = authApiClient;
    }

    private AuthApiClient AuthApiClient { get; }

    public string GetPendingAuthenticationMessage()
    {
        return "Login web estruturado. A autenticacao real sera conectada a API na UE-15.02.";
    }
}
