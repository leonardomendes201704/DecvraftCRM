using System.ComponentModel.DataAnnotations;

namespace Platform.Web.ViewModels.Account;

public sealed class LoginViewModel
{
    [Required(ErrorMessage = "Informe o endereco da API.")]
    [Url(ErrorMessage = "Informe uma URL valida para a API.")]
    public string ApiBaseUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o tenant.")]
    public string TenantSlug { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail valido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a senha.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
