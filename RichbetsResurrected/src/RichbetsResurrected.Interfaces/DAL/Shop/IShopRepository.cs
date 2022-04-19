using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Interfaces.DAL.Shop;

public interface IShopRepository
{
    List<ActiveItem> GetActiveItems();
    List<Category> GetCategories();
    List<SubCategory> GetSubCategories();
    List<ConsumableItem> GetConsumableItems();
    List<Discount> GetDiscounts();
    List<Item> GetItems();
    List<UserItem> GetUserItems();
    List<ItemType> GetItemTypes();
    void AddActiveItem(ActiveItem activeItem);
    void AddCategory(Category category);
    void AddConsumableItem(ConsumableItem consumableItem);
    void AddDiscount(Discount discount);
    void AddItem(Item item);
    void AddUserItem(UserItem userItem);
    void AddSubCategory(SubCategory subCategory);
    void AddItemType(ItemType itemType);
    List<SubCategory> GetSubCategoriesByCategory(Category category);
    List<Item> GetItemsBySubCategory(SubCategory subCategory);
    ActiveItem? GetActiveItemByIds(int richbetUserId, int itemId);
    Category? GetCategoryById(int categoryId);
    SubCategory? GetSubCategoryById(int subCategoryId);
    ConsumableItem? GetConsumableItemByItemId(int itemId);
    Discount? GetDiscountByItemId(int itemId);
    Item? GetItemById(int itemId);
    UserItem? GetUserItemByIds(int richbetUserId, int itemId);
    ItemType? GetItemTypeByItemId(int itemId);
}