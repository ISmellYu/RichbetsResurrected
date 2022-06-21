using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Identity.Repositories;
using RichbetsResurrected.Interfaces.Shop;
using RichbetsResurrected.Services.Shop;

namespace RichbetsResurrected.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Authorize]
public class ClientController : Controller
{
    public IActionResult SpaceRun()
    {
        return View();
    }

}