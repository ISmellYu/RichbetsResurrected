using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class Item
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName = "VARCHAR(256)")]
    public string Name { get; set; }
    [Column(TypeName = "VARCHAR(256)")]
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int Price { get; set; }
    public int AvailableQuantity { get; set; }
    public bool IsAvailable { get; set; }

    public int SubCategoryId { get; set; }
    public SubCategory SubCategory { get; set; }
    
    public ItemType ItemType { get; set; }
    
    public ConsumableItem? ConsumableItem { get; set; }
    
    public Discount? Discount { get; set; }
    
    public IEnumerable<ActiveItem> ActivatedItems { get; set; }
    public IEnumerable<UserItem> UserItems { get; set; }
}