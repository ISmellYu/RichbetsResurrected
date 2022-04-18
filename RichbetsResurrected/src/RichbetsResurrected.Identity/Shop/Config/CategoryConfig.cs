using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Identity.Shop.Config;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.ToTable("categories");
    }
}