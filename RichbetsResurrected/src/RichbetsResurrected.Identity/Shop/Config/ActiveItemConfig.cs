using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using SQLitePCL;

namespace RichbetsResurrected.Identity.Shop.Config;

public class ActiveItemConfig : IEntityTypeConfiguration<ActiveItem>
{

    public void Configure(EntityTypeBuilder<ActiveItem> builder)
    {
        builder.HasKey(ai => ai.Id);
        builder.ToTable("activeItems");
    }
}