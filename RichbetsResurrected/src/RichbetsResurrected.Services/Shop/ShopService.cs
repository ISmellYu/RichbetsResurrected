using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Entities.Shop;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.DAL.Shop;
using RichbetsResurrected.Interfaces.Shop;

namespace RichbetsResurrected.Services.Shop;

public class ShopService : IShopService
{
    private readonly IShopRepository _shopRepository;
    private readonly IRichbetRepository _richbetRepository;

    public ShopService(IShopRepository shopRepository, IRichbetRepository richbetRepository)
    {
        _shopRepository = shopRepository;
        _richbetRepository = richbetRepository;
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
        var totalPrice = discounts.Where(discount => discount.ItemId == item.Id && discount.Quantity > 0)
            .Aggregate(item.Price, (current, discount) => current - (current * discount.DiscountPercentage));
        return totalPrice;
    }

    public ActiveItem? GetActiveItemByIds(int identityUserId, int itemId)
    {
        return _shopRepository.GetActiveItemByIds(identityUserId, itemId);
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

    public UserItem? GetUserItemByIds(int identityUserId, int itemId)
    {
        return _shopRepository.GetUserItemByIds(identityUserId, itemId);
    }

    public ItemType? GetItemTypeByItemId(int itemId)
    {
        return _shopRepository.GetItemTypeByItemId(itemId);
    }

    public async Task<BuyResult> BuyItemAsync(int identityUserId, int itemId)
    {
        var richbetUser = await _richbetRepository.GetRichbetUserAsync(identityUserId);
        var item = _shopRepository.GetItemById(itemId);

        if (item == null)
        {
            return new BuyResult
            {
                IsSuccess = false,
                Error = new ShopError {Message = "Item with id " + itemId + " does not exist"},
                Item = item
            };
        }

        if (!IsAvailableForPurchase(item))
        {
            return new BuyResult
            {
                IsSuccess = false,
                Error = new ShopError {Message = "This item is not available for purchase"},
                Item = item
            };
        }

        if (!IsEligibleForPurchase(item, identityUserId))
        {
            return new BuyResult
            {
                IsSuccess = false,
                Error = new ShopError {Message = "You are not eligible for purchase this item"},
                Item = item
            };
        }

        var totalPrice = CalculateTotalPriceForItem(item);

        if (richbetUser.Points - totalPrice < 0)
        {
            return new BuyResult
            {
                IsSuccess = false,
                Error = new ShopError {Message = "You do not have enough points to buy this item"},
                Item = item
            };
        }

        var sale = GetDiscountByItemId(itemId);
        if (sale != null)
        {
            UseDiscount(sale);
        }

        var boughtItem = BuyItem(item, totalPrice, identityUserId);

        return new BuyResult {IsSuccess = true, Error = null, Item = boughtItem};
    }

    public bool IsOnSale(int itemId)
    {
        return _shopRepository.GetDiscountByItemId(itemId) != null;
    }

    public bool IsAvailableForPurchase(Item item)
    {
        switch (item.IsAvailable)
        {
            case true when item.AvailableQuantity == -1:
            case true when item.AvailableQuantity > 0:
                return true;
            default:
                return false;
        }
    }

    public bool IsEligibleForPurchase(Item item, int identityUserId)
    {
        var itemType = _shopRepository.GetItemTypeByItemId(item.Id);
        if (itemType == null)
        {
            return false;
        }

        if (itemType.IsUnique)
        {
            var hasitem = HasItem(identityUserId, item.Id);
            return !hasitem;
        }

        return true;
    }

    public void UseDiscount(Discount discount)
    {
        if (discount.Quantity != -1 && discount.Quantity > 1)
        {
            discount.Quantity--;
            _shopRepository.UpdateDiscount(discount);
        }
        else if (discount.Quantity == 1)
        {
            _shopRepository.RemoveDiscount(discount);
        }
    }

    private Item BuyItem(Item item, int pointsToRemove, int identityUserId)
    {
        if (item.AvailableQuantity > 0)
        {
            item.AvailableQuantity--;
        }

        if (item.AvailableQuantity == 0)
        {
            item.IsAvailable = false;
        }

        _shopRepository.UpdateItem(item);

        _richbetRepository.RemovePointsFromUserAsync(identityUserId, pointsToRemove);
        if (HasItem(identityUserId, item.Id))
        {
            _shopRepository.UpdateUserItem(new UserItem
            {
                RichbetUserId = identityUserId,
                ItemId = item.Id,
                Quantity = GetUserItemByIds(identityUserId, item.Id).Quantity + 1
            });
        }
        else
        {
            _shopRepository.AddUserItem(new UserItem {RichbetUserId = identityUserId, ItemId = item.Id, Quantity = 1});
        }


        return item;
    }

    private bool HasItem(int identityUserId, int itemId)
    {
        return _shopRepository.GetUserItemByIds(identityUserId, itemId) != null;
    }
}
