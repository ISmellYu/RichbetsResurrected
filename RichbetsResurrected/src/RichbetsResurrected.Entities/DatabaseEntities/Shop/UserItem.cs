namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class UserItem
{
    public int ItemId { get; set; }
    public int RichbetUserId { get; set; }
    public int Quantity { get; set; }
}