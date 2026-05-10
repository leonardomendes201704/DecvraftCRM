using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Web.Services;

namespace Platform.Web.Controllers;

[Authorize]
public sealed class CrmController : Controller
{
    private readonly CrmWebService _crmWebService;

    public CrmController(CrmWebService crmWebService)
    {
        _crmWebService = crmWebService;
    }

    public IActionResult Index()
    {
        return View(_crmWebService.GetHome());
    }
}
