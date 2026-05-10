using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Web.Models;
using Platform.Web.Services;

namespace Platform.Web.Controllers;

[Authorize]
public sealed class DashboardController : Controller
{
    private readonly DashboardWebService _dashboardWebService;

    public DashboardController(DashboardWebService dashboardWebService)
    {
        _dashboardWebService = dashboardWebService;
    }

    public IActionResult Index()
    {
        return View(_dashboardWebService.GetInitialDashboard());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
