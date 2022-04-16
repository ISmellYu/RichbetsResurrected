using Microsoft.AspNetCore.Mvc;

namespace RichbetsResurrected.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ItemshopController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}