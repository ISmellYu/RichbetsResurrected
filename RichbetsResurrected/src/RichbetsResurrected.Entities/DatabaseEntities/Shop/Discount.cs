using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class Discount
{
    public int Quantity { get; set; }
    public int DiscountPercentage { get; set; }
    [JsonIgnore]
    public int ItemId { get; set; }
    [JsonIgnore]
    public Item Item { get; set; }
}