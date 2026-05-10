using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Platform.Web.Services;
using Platform.Web.ViewModels.Account;

namespace Platform.Web.Pages.Account;

public sealed class LoginModel : PageModel
{
    private readonly AuthWebService _authWebService;

    public LoginModel(AuthWebService authWebService)
    {
        _authWebService = authWebService;
    }

    [BindProperty]
    public LoginViewModel Input { get; set; } = new();

    public string Message { get; private set; } = string.Empty;

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Message = _authWebService.GetPendingAuthenticationMessage();
        return Page();
    }
}
