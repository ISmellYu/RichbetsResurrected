using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichbetsResurrected.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class DashboardController : Controller
{
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
