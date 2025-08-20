using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Identity.Shop.Config;

public class ItemConfig : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        //builder.HasOne<Category>().WithMany(d => d.Items).HasForeignKey(p => p.CategoryId).IsRequired();
        //builder.HasOne<ConsumableItem>().WithOne().HasForeignKey<Item>(p => p.ConsumableItemId);
        // builder.HasOne<Discount>().WithOne().HasForeignKey<Item>(p => p.DiscountId);
        //builder.HasOne<Discount>().WithOne().HasForeignKey<Item>(p => p.DiscountId);
        builder.ToTable("items");
    }
}
