﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Entities.Shop;
using RichbetsResurrected.Interfaces.Inventory;
using RichbetsResurrected.Interfaces.Shop;

namespace RichbetsResurrected.Web.ApiController;
[Authorize]
[Route("[controller]/[action]")]
[ApiController]
public class ItemshopController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IShopService _shopService;
    private readonly IInventoryService _inventoryService;
    public ItemshopController(UserManager<AppUser> userManager, IShopService shopService, IInventoryService inventoryService)
    {
        _userManager = userManager;
        _shopService = shopService;
        _inventoryService = inventoryService;

    }
    
    [HttpPost]
    public async Task<BuyResult> BuyItem(string itemId)
    {
        var user = await _userManager.GetUserAsync(User);
        var result = await _shopService.BuyItemAsync(user.Id, Convert.ToInt32(itemId));
        if (result.Item != null)
        {
            bool IsDesignItem(ItemType type)
            {
                if (type.IsNicknameAnimation || type.IsNicknameBanner || type.IsNicknameEffect || type.IsNicknamePattern)
                {
                    return true;
                }
                return false;
            }

            var items = _inventoryService.GetEquippedItems(user.Id);
            var resultItemType = _shopService.GetItemTypeByItemId(result.Item.Id);
            var resultItemIsDesign = IsDesignItem(resultItemType);
            var equipped = items.SingleOrDefault(item =>
            {
                var itemType = _shopService.GetItemTypeByItemId(item.Id);
                if (IsDesignItem(itemType) && resultItemIsDesign)
                {
                    return true;
                }
                return false;
            });
            if (equipped != null)
            {
                _inventoryService.UnequipItem(user.Id, equipped.Id);
                _inventoryService.RemoveItem(user.Id, equipped.Id);
            }
            _inventoryService.EquipItem(user.Id, result.Item.Id);
        }
        return result;
    }
}