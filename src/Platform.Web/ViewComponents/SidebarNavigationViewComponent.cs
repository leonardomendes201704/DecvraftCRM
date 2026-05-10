using Microsoft.AspNetCore.Mvc;
using Platform.Web.Constants;
using Platform.Web.ViewModels.Navigation;

namespace Platform.Web.ViewComponents;

public sealed class SidebarNavigationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var currentController = Convert.ToString(RouteData.Values["controller"]) ?? string.Empty;

        var items = new[]
        {
            Create("Dashboard", WebRouteNames.DashboardController, NavigationIconKey.Dashboard, currentController),
            Create("CRM", WebRouteNames.CrmController, NavigationIconKey.Crm, currentController),
            Create("Financeiro", WebRouteNames.FinanceController, NavigationIconKey.Finance, currentController),
            Create("Organizacao", WebRouteNames.OrganizationController, NavigationIconKey.Organization, currentController)
        };

        return View(new SidebarNavigationViewModel(items));
    }

    private static NavigationItemViewModel Create(
        string label,
        string controller,
        NavigationIconKey iconKey,
        string currentController)
    {
        return new NavigationItemViewModel(
            label,
            controller,
            WebRouteNames.IndexAction,
            iconKey,
            string.Equals(controller, currentController, StringComparison.OrdinalIgnoreCase));
    }
}
