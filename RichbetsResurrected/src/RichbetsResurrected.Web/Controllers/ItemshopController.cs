using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Entities.Shop;
using RichbetsResurrected.Interfaces.Shop;
using RichbetsResurrected.Web.ViewModels;

namespace RichbetsResurrected.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Authorize]
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
        
        return View(new CustomStylingViewModel
        {
            SubCategories = subCategories.ToList()
        });
    }

    public IActionResult LureModules()
    {
        //return View();
        return RedirectToAction("Index", "Error");
    }

    public IActionResult MuteBypass()
    {
        //return View();
        return RedirectToAction("Index", "Error");
    }

    public IActionResult BuildingItems()
    {
        //return View();
        return RedirectToAction("Index", "Error");
    }
    
    
}