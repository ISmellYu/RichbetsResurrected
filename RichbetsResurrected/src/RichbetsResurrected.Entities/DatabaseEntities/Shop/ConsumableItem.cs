using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class ConsumableItem
{
    public bool IsTimed { get; set; }
    public int? TimeInSeconds { get; set; }
    
    [JsonIgnore]
    public int ItemId { get; set; }
    [JsonIgnore]
    public Item Item { get; set; }
}