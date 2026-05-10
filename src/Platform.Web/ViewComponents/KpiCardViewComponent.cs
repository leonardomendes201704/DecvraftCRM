using Microsoft.AspNetCore.Mvc;
using Platform.Web.ViewModels.Components;

namespace Platform.Web.ViewComponents;

public sealed class KpiCardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string label, string value, string context)
    {
        return View(new KpiCardViewModel(label, value, context));
    }
}
