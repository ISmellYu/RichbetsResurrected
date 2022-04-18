using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Identity.Shop.Config;

public class ItemConfig : IEntityTypeConfiguration<Item>
{

    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(i => i.Id);
        builder.ToTable("items");
    }
}