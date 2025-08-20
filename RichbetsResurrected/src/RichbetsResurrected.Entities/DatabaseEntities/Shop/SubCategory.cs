using System.ComponentModel.DataAnnotations.Schema;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class SubCategory
{
    public int Id { get; set; }
    [Column(TypeName = "VARCHAR(256)")] public string Name { get; set; }
    [Column(TypeName = "VARCHAR(256)")] public string Description { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public IEnumerable<Item> Items { get; set; }
}
