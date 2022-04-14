using Microsoft.AspNetCore.Mvc;

namespace RichbetsResurrected.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class GamesController : Controller
{
    public IActionResult Roulette()
    {
        return View();
    }

    public IActionResult Test()
    {
        return View();
    }
}