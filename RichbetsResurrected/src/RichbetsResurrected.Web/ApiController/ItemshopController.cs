using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Entities.Identity.Models;
using RichbetsResurrected.Entities.Shop;
using RichbetsResurrected.Interfaces.Shop;

namespace RichbetsResurrected.Web.ApiController;
[Authorize]
[Route("[controller]/[action]")]
[ApiController]
public class ItemshopController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IShopService _shopService;
    public ItemshopController(UserManager<AppUser> userManager, IShopService shopService)
    {
        _userManager = userManager;
        _shopService = shopService;

    }
    
    [HttpPost]
    public async Task<BuyResult> BuyItem(string itemId)
    {
        var user = await _userManager.GetUserAsync(User);
        var result = await _shopService.BuyItemAsync(user.Id, Convert.ToInt32(itemId));
        return result;
    }
}