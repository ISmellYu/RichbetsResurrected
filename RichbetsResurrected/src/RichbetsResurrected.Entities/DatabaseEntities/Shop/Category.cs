using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName = "VARCHAR(256)")]
    public string Name { get; set; }
    [Column(TypeName = "VARCHAR(256)")]
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    
    public IEnumerable<SubCategory> SubCategories { get; set; }
}