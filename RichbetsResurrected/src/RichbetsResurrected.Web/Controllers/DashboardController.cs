using Microsoft.AspNetCore.Mvc;

namespace RichbetsResurrected.Web.Controllers;

public class DashboardController : Controller
{
    [Route("/Error")]
    public IActionResult Index()
    {
        // error
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}