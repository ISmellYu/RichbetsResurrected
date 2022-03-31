using Microsoft.AspNetCore.Mvc;

namespace RichbetsResurrected.Web.Controllers;

[Route("errors")]
public class ErrorController : Controller
{
    [Route("{statusCode:int}")]
    public IActionResult Index(int statusCode)
    {
        if (statusCode == 404)
        {
            return View("NotFound");
        }

        if (statusCode == 403)
        {
            return View("AccessDenied");
        }
        
        return View("Error");
    }
}