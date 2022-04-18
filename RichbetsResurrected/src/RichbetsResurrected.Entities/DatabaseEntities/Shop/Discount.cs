namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class Discount
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int DiscountPercentage { get; set; }
    public int? CategoryId { get; set; }
    public int? ItemId { get; set; }
}