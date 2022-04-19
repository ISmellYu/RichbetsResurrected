using Microsoft.EntityFrameworkCore;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Identity.Contexts;
using RichbetsResurrected.Interfaces.DAL.Shop;

namespace RichbetsResurrected.Identity.Repositories;

public class ShopRepository : IShopRepository
{
    private AppDbContext _context;
    
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
    
    public void AddActiveItem(ActiveItem activeItem)
    {
        _context.ActiveItems.Add(activeItem);
        _context.SaveChanges();
    }
    
    public void AddCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }
    
    public void AddConsumableItem(ConsumableItem consumableItem)
    {
        _context.ConsumableItems.Add(consumableItem);
        _context.SaveChanges();
    }
    
    public void AddDiscount(Discount discount)
    {
        _context.Discounts.Add(discount);
        _context.SaveChanges();
    }
    
    public void AddItem(Item item)
    {
        _context.Items.Add(item);
        _context.SaveChanges();
    }
    
    public void AddUserItem(UserItem userItem)
    {
        _context.UserItems.Add(userItem);
        _context.SaveChanges();
    }
}