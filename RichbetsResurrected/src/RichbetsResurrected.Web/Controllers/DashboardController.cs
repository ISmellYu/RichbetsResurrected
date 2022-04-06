using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Infrastructure.Games.Roulette;

namespace RichbetsResurrected.Web.Controllers;

public class DashboardController : Controller
{
    public DashboardController()
    {
    }
    
    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize(AuthenticationSchemes = DiscordAuthenticationDefaults.AuthenticationScheme)]
    public IActionResult Huj()
    {
        return RedirectToRoute("Index", "Home");
    }
}