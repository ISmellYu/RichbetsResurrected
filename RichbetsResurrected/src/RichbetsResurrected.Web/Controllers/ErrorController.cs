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
        
        return View("Error");
    }
}