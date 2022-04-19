using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Identity.Repositories;
using RichbetsResurrected.Services.Shop;

namespace RichbetsResurrected.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class GamesController : Controller
{
    private readonly ShopService _shopService;
    public GamesController(ShopService shopService)
    {
        _shopService = shopService;
    }
    [Authorize]
    public IActionResult Roulette()
    {
        return View();
    }

    public IActionResult Test()
    {
        var x = _shopService.GetItems();
        return View();
    }
}