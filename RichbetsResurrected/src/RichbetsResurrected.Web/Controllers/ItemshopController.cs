using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Interfaces.Shop;
using RichbetsResurrected.Web.ViewModels;

namespace RichbetsResurrected.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ItemshopController : Controller
{
    private readonly IShopService _shopService;

    public ItemshopController(IShopService shopService)
    {
        _shopService = shopService;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CustomStyling()
    {
        var allCategories = _shopService.GetCategories();
        var correctCategory = allCategories.FirstOrDefault(p => p.Name == "Styling");
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
}