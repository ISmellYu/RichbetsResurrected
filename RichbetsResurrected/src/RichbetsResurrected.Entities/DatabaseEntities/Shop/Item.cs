using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class Item
{
    [Key] [JsonIgnore] public int Id { get; set; }
    [Column(TypeName = "VARCHAR(256)")] public string Name { get; set; }
    [Column(TypeName = "VARCHAR(256)")] public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int Price { get; set; }
    public int AvailableQuantity { get; set; }
    public bool IsAvailable { get; set; }

    [JsonIgnore] public int SubCategoryId { get; set; }
    [JsonIgnore] public SubCategory SubCategory { get; set; }

    public ItemType ItemType { get; set; }

    public ConsumableItem? ConsumableItem { get; set; }

    public Discount? Discount { get; set; }
    [JsonIgnore] public IEnumerable<ActiveItem> ActivatedItems { get; set; }
    [JsonIgnore] public IEnumerable<UserItem> UserItems { get; set; }
}
