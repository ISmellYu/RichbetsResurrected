namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int Price { get; set; }
    public int AvailableQuantity { get; set; }
    public bool IsAvailable { get; set; }
    public int CategoryId { get; set; }
    public int? ConsumableItemId { get; set; }
}