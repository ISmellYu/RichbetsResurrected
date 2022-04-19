using System;
using System.ComponentModel.DataAnnotations;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Web.ViewModels;

public class CustomStylingViewModel
{
    public List<SubCategory> SubCategories { get; set; } 
}