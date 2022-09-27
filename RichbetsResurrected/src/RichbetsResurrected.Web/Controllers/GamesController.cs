using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Identity.Repositories;
using RichbetsResurrected.Interfaces.Shop;
using RichbetsResurrected.Services.Shop;

namespace RichbetsResurrected.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Authorize]
public class GamesController : Controller
{
  private readonly IShopService _shopService;
  public GamesController(IShopService shopService)
  {
    _shopService = shopService;
  }

  public IActionResult Roulette()
  {
    return View();
  }

  public IActionResult Spacerun()
  {
    return View();
  }

  public IActionResult Crash()
  {
    return View();
  }

  public IActionResult SlotsClassic()
  {
    return View();
  }
}