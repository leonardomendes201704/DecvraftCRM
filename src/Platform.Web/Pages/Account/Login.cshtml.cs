using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Platform.Web.Constants;
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

    public string ErrorMessage { get; private set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction(WebRouteNames.IndexAction, WebRouteNames.DashboardController);
        }

        return Page();
    }

    public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _authWebService.LoginAsync(Input, cancellationToken);
        if (!result.Succeeded || result.Login is null || result.CurrentUser is null)
        {
            ErrorMessage = result.ErrorMessage ?? AuthErrorMessages.InvalidCredentials;
            return Page();
        }

        _authWebService.TryBuildApiBaseUrl(Input.ApiBaseUrl, out var apiBaseUrl);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            _authWebService.BuildPrincipal(result.Login, result.CurrentUser, apiBaseUrl),
            _authWebService.BuildAuthenticationProperties(result.Login.ExpiresAt));

        return RedirectToAction(WebRouteNames.IndexAction, WebRouteNames.DashboardController);
    }
}
