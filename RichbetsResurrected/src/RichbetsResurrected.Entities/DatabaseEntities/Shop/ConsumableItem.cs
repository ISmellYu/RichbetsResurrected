namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class ConsumableItem
{
    public int Id { get; set; }
    public bool IsTimed { get; set; }
    public int TimeInSeconds { get; set; }
    public int ItemId { get; set; }
}