using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Web.Services;

namespace Platform.Web.Controllers;

[Authorize]
public sealed class FinanceController : Controller
{
    private readonly FinanceWebService _financeWebService;

    public FinanceController(FinanceWebService financeWebService)
    {
        _financeWebService = financeWebService;
    }

    public IActionResult Index()
    {
        return View(_financeWebService.GetHome());
    }
}
