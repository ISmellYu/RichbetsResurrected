using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.DAL.Shop;

namespace RichbetsResurrected.Services.Client;

public class InventoryService
{
    private readonly IShopRepository _shopRepository;
    private readonly IRichbetRepository _richbetRepository;
    public InventoryService(IShopRepository shopRepository, IRichbetRepository richbetRepository)
    {
        _shopRepository = shopRepository;
        _richbetRepository = richbetRepository;
    }

    public Inventory GetInventory(int identityUserId)
    {
        var activeItems = _shopRepository.GetActiveItems()
            .Where(p => p.RichetUserId == identityUserId)
            .ToList();
        
        var userItems = _shopRepository.GetUserItems();
        var equippedItems = userItems
            .Where(p => p.RichbetUserId == identityUserId && p.State == ItemState.Active)
            .Select(p => _shopRepository.GetItemById(p.ItemId))
            .ToList();
        
        var allItems = userItems
            .Where(p => p.RichbetUserId == identityUserId)
            .ToList();
        foreach (var userItem in allItems)
        {
            userItem.Item = _shopRepository.GetItemById(userItem.ItemId);
        }
        var inv = new Inventory()
        {
            ActiveItems = activeItems, EquippedItems = equippedItems, Items = allItems
        };
        return inv;
    }

    public void EquipItem(int identityUserId, int itemId)
    {
        var userItem = _shopRepository.GetUserItemByIds(identityUserId, itemId);
        if (userItem == null) return;
        var equippable = _shopRepository.GetItemTypeByItemId(userItem.ItemId).IsEquippable;
        if (equippable)
        {
            userItem.State = ItemState.Equipped;
            _shopRepository.UpdateUserItem(userItem);
        }
    }
    
    public void UnequipItem(int identityUserId, int itemId)
    {
        var userItem = _shopRepository.GetUserItemByIds(identityUserId, itemId);
        if (userItem == null) return;
        var equippable = _shopRepository.GetItemTypeByItemId(userItem.ItemId).IsEquippable;
        if (equippable)
        {
            userItem.State = ItemState.UnEquipped;
            _shopRepository.UpdateUserItem(userItem);
        }
    }

    public bool HasItem(int identityUserId, int itemId)
    {
        var userItem = _shopRepository.GetUserItemByIds(identityUserId, itemId);
        return userItem != null;
    }
}