using System.Text.Json.Serialization;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class ConsumableItem
{
    public bool IsTimed { get; set; }
    public int? TimeInSeconds { get; set; }

    [JsonIgnore] public int ItemId { get; set; }
    [JsonIgnore] public Item Item { get; set; }
}
