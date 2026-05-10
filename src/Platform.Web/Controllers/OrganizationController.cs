using Microsoft.AspNetCore.Mvc;
using Platform.Web.Services;

namespace Platform.Web.Controllers;

public sealed class OrganizationController : Controller
{
    private readonly OrganizationWebService _organizationWebService;

    public OrganizationController(OrganizationWebService organizationWebService)
    {
        _organizationWebService = organizationWebService;
    }

    public IActionResult Index()
    {
        return View(_organizationWebService.GetHome());
    }
}
