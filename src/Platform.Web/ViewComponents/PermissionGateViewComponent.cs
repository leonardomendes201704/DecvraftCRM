using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Platform.Web.Security;

namespace Platform.Web.ViewComponents;

public sealed class PermissionGateViewComponent : ViewComponent
{
    private readonly PermissionViewPolicy _permissionViewPolicy;

    public PermissionGateViewComponent(PermissionViewPolicy permissionViewPolicy)
    {
        _permissionViewPolicy = permissionViewPolicy;
    }

    public IViewComponentResult Invoke(string permission, string fallback = "")
    {
        var allowed = _permissionViewPolicy.CanView(permission);
        return View(new HtmlString(allowed ? string.Empty : fallback));
    }
}
