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
    void RemoveActiveItem(ActiveItem activeItem);
    void UpdateActiveItem(ActiveItem activeItem);
    void AddCategory(Category category);
    void RemoveCategory(Category category);
    void UpdateCategory(Category category);
    void AddConsumableItem(ConsumableItem consumableItem);
    void RemoveConsumableItem(ConsumableItem consumableItem);
    void UpdateConsumableItem(ConsumableItem consumableItem);
    void AddDiscount(Discount discount);
    void RemoveDiscount(Discount discount);
    void UpdateDiscount(Discount discount);
    void AddItem(Item item);
    void RemoveItem(Item item);
    void UpdateItem(Item item);
    void AddUserItem(UserItem userItem);
    void RemoveUserItem(UserItem userItem);
    void UpdateUserItem(UserItem userItem);
    void AddSubCategory(SubCategory subCategory);
    void RemoveSubCategory(SubCategory subCategory);
    void UpdateSubCategory(SubCategory subCategory);
    void AddItemType(ItemType itemType);
    void RemoveItemType(ItemType itemType);
    void UpdateItemType(ItemType itemType);
    List<SubCategory> GetSubCategoriesByCategory(Category category);
    List<Item> GetItemsBySubCategory(SubCategory subCategory);
    ActiveItem? GetActiveItemByIds(int identityUserId, int itemId);
    Category? GetCategoryById(int categoryId);
    SubCategory? GetSubCategoryById(int subCategoryId);
    ConsumableItem? GetConsumableItemByItemId(int itemId);
    Discount? GetDiscountByItemId(int itemId);
    Item? GetItemById(int itemId);
    UserItem? GetUserItemByIds(int identityUserId, int itemId);
    ItemType? GetItemTypeByItemId(int itemId);
}