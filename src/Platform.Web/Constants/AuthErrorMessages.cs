namespace Platform.Web.Constants;

public static class AuthErrorMessages
{
    public const string InvalidCredentials = "Tenant, e-mail ou senha invalidos.";
    public const string ApiUnavailable = "Nao foi possivel conectar na API informada.";
    public const string InvalidApiBaseUrl = "Informe uma URL valida para a API.";
    public const string CurrentUserUnavailable = "Login recebido, mas nao foi possivel carregar o usuario atual.";
}
