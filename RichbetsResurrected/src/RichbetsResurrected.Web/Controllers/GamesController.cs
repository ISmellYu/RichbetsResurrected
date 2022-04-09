using Microsoft.AspNetCore.Mvc;

namespace RichbetsResurrected.Web.Controllers;


public class GamesController : Controller
{
    public IActionResult Roulette()
    {
        return View();
    }
}