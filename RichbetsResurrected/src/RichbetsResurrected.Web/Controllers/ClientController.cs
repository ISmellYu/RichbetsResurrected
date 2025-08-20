using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
