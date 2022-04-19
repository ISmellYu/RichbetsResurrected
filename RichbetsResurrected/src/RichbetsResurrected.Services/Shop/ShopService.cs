using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Interfaces.Shop;

namespace RichbetsResurrected.Services.Shop;

public class ShopService
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

    public int CalculateTotalPriceForItem(Item item)
    {
        var discounts = _shopRepository.GetDiscounts();
        var totalPrice = discounts.Where(discount => discount.ItemId == item.Id)
            .Aggregate(item.Price, (current, discount) => current - (current * discount.DiscountPercentage));
        return totalPrice;
    }
    
    
}