using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Interfaces.DAL.Shop;
using RichbetsResurrected.Interfaces.Shop;

namespace RichbetsResurrected.Services.Shop;

public class ShopService : IShopService
{
    private readonly IShopRepository _shopRepository;
    public ShopService(IShopRepository shopRepository)
    {
        _shopRepository = shopRepository;
    }
    
    public List<Item> GetItems()
    {
        return _shopRepository.GetItems();
    }
    
    public List<Category> GetCategories()
    {
        return _shopRepository.GetCategories();
    }
    
    public List<SubCategory> GetSubCategories()
    {
        return _shopRepository.GetSubCategories();
    }
    
    public List<Discount> GetDiscounts()
    {
        return _shopRepository.GetDiscounts();
    }
    
    public List<ActiveItem> GetActiveItems()
    {
        return _shopRepository.GetActiveItems();
    }
    
    public List<ConsumableItem> GetConsumableItems()
    {
        return _shopRepository.GetConsumableItems();
    }
    
    public List<UserItem> GetUserItems()
    {
        return _shopRepository.GetUserItems();
    }
    
    public List<ItemType> GetItemTypes()
    {
        return _shopRepository.GetItemTypes();
    }

    public int CalculateTotalPriceForItem(Item item)
    {
        var discounts = _shopRepository.GetDiscounts();
        var totalPrice = discounts.Where(discount => discount.ItemId == item.Id)
            .Aggregate(item.Price, (current, discount) => current - (current * discount.DiscountPercentage));
        return totalPrice;
    }
    public ActiveItem? GetActiveItemByIds(int richbetUserId, int itemId)
    {
        return _shopRepository.GetActiveItemByIds(richbetUserId, itemId);
    }
    public Category? GetCategoryById(int categoryId)
    {
        return _shopRepository.GetCategoryById(categoryId);
    }
    public SubCategory? GetSubCategoryById(int subCategoryId)
    {
        return _shopRepository.GetSubCategoryById(subCategoryId);
    }
    public ConsumableItem? GetConsumableItemByItemId(int itemId)
    {
        return _shopRepository.GetConsumableItemByItemId(itemId);
    }
    public Discount? GetDiscountByItemId(int itemId)
    {
        return _shopRepository.GetDiscountByItemId(itemId);
    }
    public Item? GetItemById(int itemId)
    {
        return _shopRepository.GetItemById(itemId);
    }
    public UserItem? GetUserItemByIds(int richbetUserId, int itemId)
    {
        return _shopRepository.GetUserItemByIds(richbetUserId, itemId);
    }
    public ItemType? GetItemTypeByItemId(int itemId)
    {
        return _shopRepository.GetItemTypeByItemId(itemId);
    }

}