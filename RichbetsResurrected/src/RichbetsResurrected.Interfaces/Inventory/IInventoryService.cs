namespace RichbetsResurrected.Interfaces.DAL.Inventory;

public interface IInventoryService
{
    Entities.Client.Inventory GetInventory(int identityUserId);
    void EquipItem(int identityUserId, int itemId);
    void UnequipItem(int identityUserId, int itemId);
    bool HasItem(int identityUserId, int itemId);
}