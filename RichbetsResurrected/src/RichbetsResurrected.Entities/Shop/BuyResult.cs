using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Entities.Shop;

public class BuyResult
{
    public bool IsSuccess { get; set; }
    public ShopError? Error { get; set; }
    public Item? Item { get; set; }
}