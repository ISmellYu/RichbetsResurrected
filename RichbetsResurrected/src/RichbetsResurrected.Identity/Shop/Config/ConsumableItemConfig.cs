using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Identity.Shop.Config;

public class ConsumableItemConfig : IEntityTypeConfiguration<ConsumableItem>
{
    public void Configure(EntityTypeBuilder<ConsumableItem> builder)
    {
        builder.HasKey(u => u.ItemId);
        //builder.HasOne<Item>().WithOne().HasForeignKey<ConsumableItem>(p => p.ItemId).IsRequired();
        builder.ToTable("consumableItems");
    }
}
