using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.DAL.Shop;

namespace RichbetsResurrected.Services.Client;

public class InventoryService
{
    private readonly IShopRepository _shopRepository;
    private readonly IRichbetRepository _richbetRepository;
    public InventoryService(IShopRepository shopRepository, IRichbetRepository richbetRepository)
    {
        _shopRepository = shopRepository;
        _richbetRepository = richbetRepository;
    }
    
}