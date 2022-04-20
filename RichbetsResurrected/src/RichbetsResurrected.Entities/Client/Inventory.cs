using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Entities.Client;

public class Inventory
{
    public List<Item> Items { get; set; }
    public List<ActiveItem> ActiveItems { get; set; }
    public List<Item> EquippedItems { get; set; }
}