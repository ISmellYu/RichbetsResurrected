using Microsoft.AspNetCore.Mvc;

namespace RichbetsResurrected.Web.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}