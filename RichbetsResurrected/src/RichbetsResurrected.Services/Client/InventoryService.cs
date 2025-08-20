using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Interfaces.DAL.Shop;
using RichbetsResurrected.Interfaces.Inventory;

namespace RichbetsResurrected.Services.Client;

public class InventoryService : IInventoryService
{
    private readonly IShopRepository _shopRepository;

    public InventoryService(IShopRepository shopRepository)
    {
        _shopRepository = shopRepository;
    }

    public Inventory GetInventory(int identityUserId)
    {
        var inv = new Inventory
        {
            ActiveItems = GetActiveItems(identityUserId),
            EquippedItems = GetEquippedItems(identityUserId),
            Items = GetUserItemsWithAll(identityUserId)
        };
        return inv;
    }

    public void EquipItem(int identityUserId, int itemId)
    {
        var userItem = _shopRepository.GetUserItemByIds(identityUserId, itemId);
        if (userItem == null)
        {
            return;
        }

        var itemType = _shopRepository.GetItemTypeByItemId(userItem.ItemId);
        if (itemType == null)
        {
            return;
        }

        if (itemType.IsEquippable)
        {
            userItem.State = ItemState.Equipped;
            _shopRepository.UpdateUserItem(userItem);
        }
    }

    public void UnequipItem(int identityUserId, int itemId)
    {
        var userItem = _shopRepository.GetUserItemByIds(identityUserId, itemId);
        if (userItem == null)
        {
            return;
        }

        var equippable = _shopRepository.GetItemTypeByItemId(userItem.ItemId).IsEquippable;
        if (equippable)
        {
            userItem.State = ItemState.UnEquipped;
            _shopRepository.UpdateUserItem(userItem);
        }
    }

    public void RemoveItem(int identityUserId, int itemId)
    {
        var userItem = _shopRepository.GetUserItemByIds(identityUserId, itemId);
        if (userItem == null)
        {
            return;
        }

        _shopRepository.RemoveUserItem(userItem);
    }

    public bool HasItem(int identityUserId, int itemId)
    {
        var userItem = _shopRepository.GetUserItemByIds(identityUserId, itemId);
        return userItem != null;
    }

    public List<Item> GetEquippedItems(int identityUserId)
    {
        var userItems = _shopRepository.GetUserItems();
        var equippedItems = userItems
            .Where(p => p.RichbetUserId == identityUserId && p.State == ItemState.Equipped)
            .Select(p => _shopRepository.GetItemById(p.ItemId))
            .ToList();
        foreach (var item in equippedItems)
        {
            item.ItemType = _shopRepository.GetItemTypeByItemId(item.Id);
        }

        return equippedItems;
    }

    public List<ActiveItem> GetActiveItems(int identityUserId)
    {
        var activeItems = _shopRepository.GetActiveItems()
            .Where(p => p.RichetUserId == identityUserId)
            .ToList();
        foreach (var item in activeItems)
        {
            item.Item = _shopRepository.GetItemById(item.ItemId);
            if (item.Item != null)
            {
                item.Item.ItemType = _shopRepository.GetItemTypeByItemId(item.ItemId);
            }
        }

        return activeItems;
    }

    public List<UserItem> GetUserItemsWithAll(int identityUserId)
    {
        var userItems = _shopRepository.GetUserItems()
            .Where(p => p.RichbetUserId == identityUserId)
            .ToList();
        foreach (var item in userItems)
        {
            item.Item = _shopRepository.GetItemById(item.ItemId);
            if (item.Item != null)
            {
                item.Item.ItemType = _shopRepository.GetItemTypeByItemId(item.ItemId);
            }
        }

        return userItems;
    }
}
