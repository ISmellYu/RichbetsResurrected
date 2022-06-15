using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Entities.Identity.Models;
using RichbetsResurrected.Interfaces.Shop;
using RichbetsResurrected.Web.ViewModels;

namespace RichbetsResurrected.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ItemshopController : Controller
{
    private readonly IShopService _shopService;
    private readonly UserManager<AppUser> _userManager;

    public ItemshopController(IShopService shopService, UserManager<AppUser> userManager)
    {
        _shopService = shopService;
        _userManager = userManager;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CustomStyling()
    {
        var allCategories = _shopService.GetCategories();
        var correctCategory = allCategories.FirstOrDefault(p => p.Name == "User Styling");
        var subCategories = _shopService.GetSubCategories()
            .Where(p => p.CategoryId == correctCategory.Id);
        foreach (var subCategory in subCategories)
        {
            subCategory.Items = _shopService.GetItems()
                .Where(p => p.SubCategoryId == subCategory.Id);
        }
        
        return View(new CustomStylingViewModel(){SubCategories = subCategories.ToList()});
    }

    public IActionResult LureModules()
    {
        return View();
    }

    public IActionResult MuteBypass()
    {
        return View();
    }

    public IActionResult BuildingItems()
    {
        return View();
    }
    
    [Authorize]
    public async Task<IActionResult> BuyItem(int itemId)
    {
        var id = Convert.ToInt32(_userManager.GetUserId(User));
        var result = await _shopService.BuyItemAsync(id, itemId);
        return Json(result);
    }
}