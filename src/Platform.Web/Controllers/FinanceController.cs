using Microsoft.AspNetCore.Mvc;
using Platform.Web.Services;

namespace Platform.Web.Controllers;

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
