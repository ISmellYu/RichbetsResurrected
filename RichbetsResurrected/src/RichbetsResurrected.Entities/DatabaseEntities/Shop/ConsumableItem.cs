using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class ConsumableItem
{
    public bool IsTimed { get; set; }
    public int? TimeInSeconds { get; set; }
    
    public int ItemId { get; set; }
    public Item Item { get; set; }
}