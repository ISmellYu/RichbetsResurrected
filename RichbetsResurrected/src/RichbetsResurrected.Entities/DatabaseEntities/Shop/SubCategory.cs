namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class SubCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public IEnumerable<Item> Items { get; set; }
}