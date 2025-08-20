using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class UserItem
{
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public int RichbetUserId { get; set; }
    public RichbetUser RichbetUser { get; set; }

    public int Quantity { get; set; }
    public ItemState State { get; set; }
}
