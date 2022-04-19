using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Identity.Shop.Config;

public class ActiveItemConfig : IEntityTypeConfiguration<ActiveItem>
{

    public void Configure(EntityTypeBuilder<ActiveItem> builder)
    {
        builder.HasKey(u => new { u.ItemId, u.RichetUserId });
        //builder.HasOne<Item>().WithOne().HasForeignKey<ActiveItem>(p => p.ItemId).IsRequired();
        //builder.HasOne<RichbetUser>().WithOne().HasForeignKey<ActiveItem>(p => p.RichetUserId).IsRequired();
        builder.ToTable("activeItems");
    }
}