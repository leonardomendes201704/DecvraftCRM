using Microsoft.AspNetCore.Mvc;
using Platform.Web.Security;
using Platform.Web.ViewModels.Shell;

namespace Platform.Web.ViewComponents;

public sealed class TopbarViewComponent : ViewComponent
{
    private readonly IWebUserContext _webUserContext;

    public TopbarViewComponent(IWebUserContext webUserContext)
    {
        _webUserContext = webUserContext;
    }

    public IViewComponentResult Invoke()
    {
        var pageTitle = Convert.ToString(ViewContext.ViewData["Title"]) ?? "Painel";

        return View(new TopbarViewModel(
            pageTitle,
            _webUserContext.DisplayName,
            _webUserContext.TenantName));
    }
}
