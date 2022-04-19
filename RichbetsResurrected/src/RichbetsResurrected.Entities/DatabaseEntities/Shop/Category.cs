﻿using System.ComponentModel.DataAnnotations;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    
    public ICollection<SubCategory> SubCategories { get; set; }
}