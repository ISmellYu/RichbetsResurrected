using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Entities.Shop;

namespace RichbetsResurrected.Interfaces.Shop;

public interface IShopService
{
    List<Item> GetItems();
    List<Category> GetCategories();
    List<SubCategory> GetSubCategories();
    List<Discount> GetDiscounts();
    List<ActiveItem> GetActiveItems();
    List<ConsumableItem> GetConsumableItems();
    List<UserItem> GetUserItems();
    List<ItemType> GetItemTypes();
    int CalculateTotalPriceForItem(Item item);
    ActiveItem? GetActiveItemByIds(int identityUserId, int itemId);
    Category? GetCategoryById(int categoryId);
    SubCategory? GetSubCategoryById(int subCategoryId);
    ConsumableItem? GetConsumableItemByItemId(int itemId);
    Discount? GetDiscountByItemId(int itemId);
    Item? GetItemById(int itemId);
    UserItem? GetUserItemByIds(int identityUserId, int itemId);
    ItemType? GetItemTypeByItemId(int itemId);
    Task<BuyResult> BuyItemAsync(int identityUserId, int itemId);
    bool IsOnSale(int itemId);
    bool IsAvailableForPurchase(Item item);
    void UseDiscount(Discount discount);
}
