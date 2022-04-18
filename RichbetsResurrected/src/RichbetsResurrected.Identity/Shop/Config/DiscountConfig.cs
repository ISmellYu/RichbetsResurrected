using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Identity.Shop.Config;

public class DiscountConfig : IEntityTypeConfiguration<Discount>
{

    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(d => d.Id);
        builder.ToTable("discounts");
    }
}