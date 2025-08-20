using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Interfaces.Inventory;

public interface IInventoryService
{
    Entities.Client.Inventory GetInventory(int identityUserId);
    void EquipItem(int identityUserId, int itemId);
    void UnequipItem(int identityUserId, int itemId);
    void RemoveItem(int identityUserId, int itemId);
    bool HasItem(int identityUserId, int itemId);
    List<Item> GetEquippedItems(int identityUserId);
    List<ActiveItem> GetActiveItems(int identityUserId);
    List<UserItem> GetUserItemsWithAll(int identityUserId);
}
