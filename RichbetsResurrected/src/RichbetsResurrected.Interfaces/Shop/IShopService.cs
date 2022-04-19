using RichbetsResurrected.Entities.DatabaseEntities.Shop;

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
    ActiveItem? GetActiveItemByIds(int richbetUserId, int itemId);
    Category? GetCategoryById(int categoryId);
    SubCategory? GetSubCategoryById(int subCategoryId);
    ConsumableItem? GetConsumableItemByItemId(int itemId);
    Discount? GetDiscountByItemId(int itemId);
    Item? GetItemById(int itemId);
    UserItem? GetUserItemByIds(int richbetUserId, int itemId);
    ItemType? GetItemTypeByItemId(int itemId);
}