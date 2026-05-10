using Microsoft.AspNetCore.Mvc;
using Platform.Web.ViewModels.Components;

namespace Platform.Web.ViewComponents;

public sealed class DataTableViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(DataTableViewModel model)
    {
        return View(model);
    }
}
