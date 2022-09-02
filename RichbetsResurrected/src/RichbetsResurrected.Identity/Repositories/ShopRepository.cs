using Microsoft.EntityFrameworkCore;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Identity.Contexts;
using RichbetsResurrected.Interfaces.DAL.Shop;

namespace RichbetsResurrected.Identity.Repositories;

public class ShopRepository : IShopRepository
{
    private readonly AppDbContext _context;
    
    public ShopRepository(AppDbContext context)
    {
        _context = context;
    }
    
    private IEnumerable<ActiveItem> ActiveItems => _context.ActiveItems.AsNoTracking();
    private IEnumerable<Category> Categories => _context.Categories.AsNoTracking();
    private IEnumerable<SubCategory> SubCategories => _context.SubCategories.AsNoTracking();
    private IEnumerable<ItemType> ItemTypes => _context.ItemTypes.AsNoTracking();
    private IEnumerable<ConsumableItem> ConsumableItems => _context.ConsumableItems.AsNoTracking();
    private IEnumerable<Discount> Discounts => _context.Discounts.AsNoTracking();
    private IEnumerable<Item> Items => _context.Items.AsNoTracking();
    private IEnumerable<UserItem> UserItems => _context.UserItems.AsNoTracking();


    public List<ActiveItem> GetActiveItems()
    {
        return ActiveItems.ToList();
    }
    
    public List<Category> GetCategories()
    {
        return Categories.ToList();
    }
    public List<SubCategory> GetSubCategories()
    {
        return SubCategories.ToList();
    }

    public List<ConsumableItem> GetConsumableItems()
    {
        return ConsumableItems.ToList();
    }
    
    public List<Discount> GetDiscounts()
    {
        return Discounts.ToList();
    }
    
    public List<Item> GetItems()
    {
        var x = _context.Items.AsNoTracking();
        return x.ToList();
    }
    
    public List<UserItem> GetUserItems()
    {
        return UserItems.ToList();
    }
    public List<ItemType> GetItemTypes()
    {
        return ItemTypes.ToList();
    }

    public void AddActiveItem(ActiveItem activeItem)
    {
        _context.ActiveItems.Add(activeItem);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void RemoveActiveItem(ActiveItem activeItem)
    {
        _context.ActiveItems.Remove(activeItem);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void UpdateActiveItem(ActiveItem activeItem)
    {
        _context.ActiveItems.Update(activeItem);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }

    public void AddCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void RemoveCategory(Category category)
    {
        _context.Categories.Remove(category);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }

    public void AddConsumableItem(ConsumableItem consumableItem)
    {
        _context.ConsumableItems.Add(consumableItem);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void RemoveConsumableItem(ConsumableItem consumableItem)
    {
        _context.ConsumableItems.Remove(consumableItem);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void UpdateConsumableItem(ConsumableItem consumableItem)
    {
        _context.ConsumableItems.Update(consumableItem);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }

    public void AddDiscount(Discount discount)
    {
        _context.Discounts.Add(discount);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void RemoveDiscount(Discount discount)
    {
        _context.Discounts.Remove(discount);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void UpdateDiscount(Discount discount)
    {
        _context.Discounts.Update(discount);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }

    public void AddItem(Item item)
    {
        _context.Items.Add(item);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void RemoveItem(Item item)
    {
        _context.Items.Remove(item);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void UpdateItem(Item item)
    {
        _context.Items.Update(item);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }

    public void AddUserItem(UserItem userItem)
    {
        _context.UserItems.Add(userItem);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void RemoveUserItem(UserItem userItem)
    {
        _context.UserItems.Remove(userItem);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void UpdateUserItem(UserItem userItem)
    {
        _context.UserItems.Update(userItem);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }

    public void AddSubCategory(SubCategory subCategory)
    {
        _context.SubCategories.Add(subCategory);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void RemoveSubCategory(SubCategory subCategory)
    {
        _context.SubCategories.Remove(subCategory);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void UpdateSubCategory(SubCategory subCategory)
    {
        _context.SubCategories.Update(subCategory);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }

    public void AddItemType(ItemType itemType)
    {
        _context.ItemTypes.Add(itemType);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void RemoveItemType(ItemType itemType)
    {
        _context.ItemTypes.Remove(itemType);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }
    public void UpdateItemType(ItemType itemType)
    {
        _context.ItemTypes.Update(itemType);
        _context.SaveChanges();
        _context.ChangeTracker.Clear(); 
    }

    public List<SubCategory> GetSubCategoriesByCategory(Category category)
    {
        return SubCategories.Where(x => x.CategoryId == category.Id).ToList();
    }
    
    public List<Item> GetItemsBySubCategory(SubCategory subCategory)
    {
        return Items.Where(x => x.SubCategoryId == subCategory.Id).ToList();
    }
    public ActiveItem? GetActiveItemByIds(int identityUserId, int itemId)
    {
        return ActiveItems.FirstOrDefault(u => u.ItemId == itemId && u.RichetUserId == identityUserId);
    }
    public Category? GetCategoryById(int categoryId)
    {
        return Categories.FirstOrDefault(x => x.Id == categoryId);
    }
    public SubCategory? GetSubCategoryById(int subCategoryId)
    {
        return SubCategories.FirstOrDefault(x => x.Id == subCategoryId);
    }
    public ConsumableItem? GetConsumableItemByItemId(int itemId)
    {
        return ConsumableItems.FirstOrDefault(x => x.ItemId == itemId);
    }
    public Discount? GetDiscountByItemId(int itemId)
    {
        return Discounts.FirstOrDefault(x => x.ItemId == itemId);
    }
    public Item? GetItemById(int itemId)
    {
        return Items.FirstOrDefault(x => x.Id == itemId);
    }
    public UserItem? GetUserItemByIds(int richbetUserId, int itemId)
    {
        return UserItems.FirstOrDefault(u => u.ItemId == itemId && u.RichbetUserId == richbetUserId);
    }
    public ItemType? GetItemTypeByItemId(int itemId)
    {
        return ItemTypes.FirstOrDefault(x => x.ItemId == itemId);
    }
}