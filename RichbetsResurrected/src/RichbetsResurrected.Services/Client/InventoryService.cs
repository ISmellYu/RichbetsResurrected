using RichbetsResurrected.Interfaces.DAL.Shop;

namespace RichbetsResurrected.Services.Client;

public class InventoryService
{
    private readonly IShopRepository _shopRepository;
    public InventoryService(IShopRepository shopRepository)
    {
        _shopRepository = shopRepository;
    }
}