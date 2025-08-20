using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Identity.Shop.Config;

public class DiscountConfig : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(u => u.ItemId);
        // builder.HasOne<Category>().WithOne()
        //     .HasForeignKey<Discount>(p => p.CategoryId);
        // builder.HasOne<Item>().WithOne()
        //     .HasForeignKey<Discount>(p => p.ItemId);
        builder.ToTable("discounts");
    }
}
