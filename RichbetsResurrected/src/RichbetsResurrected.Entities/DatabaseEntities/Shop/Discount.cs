using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class Discount
{
    public int Quantity { get; set; }
    public int DiscountPercentage { get; set; }
    
    public int ItemId { get; set; }
    public Item Item { get; set; }
}