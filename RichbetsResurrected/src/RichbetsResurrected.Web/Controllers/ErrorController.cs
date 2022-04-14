using Microsoft.AspNetCore.Mvc;

namespace RichbetsResurrected.Web.Controllers;

[Route("errors")]
public class ErrorController : Controller
{
    [Route("{statusCode:int}")]
    public IActionResult Index(int statusCode)
    {
        if (statusCode == 404) return View("NotFound");

        if (statusCode is 403 or 401) return View("AccessDenied");

        return View("Error");
    }
}