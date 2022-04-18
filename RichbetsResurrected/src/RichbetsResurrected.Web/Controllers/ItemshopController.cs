using Microsoft.AspNetCore.Mvc;

namespace RichbetsResurrected.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ItemshopController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CustomStyling()
    {
        return View();
    }

    public IActionResult LureModules()
    {
        return View();
    }

    public IActionResult MuteBypass()
    {
        return View();
    }

    public IActionResult BuildingItems()
    {
        return View();
    }
}