using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Interfaces.Shop;

public interface IShopRepository
{
    List<ActiveItem> GetActiveItems();
    List<Category> GetCategories();
    List<ConsumableItem> GetConsumableItems();
    List<Discount> GetDiscounts();
    List<Item> GetItems();
    List<UserItem> GetUserItems();
    void AddActiveItem(ActiveItem activeItem);
    void AddCategory(Category category);
    void AddConsumableItem(ConsumableItem consumableItem);
    void AddDiscount(Discount discount);
    void AddItem(Item item);
    void AddUserItem(UserItem userItem);
}