using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Identity.Shop.Config;

public class UserItemConfig : IEntityTypeConfiguration<UserItem>
{

    public void Configure(EntityTypeBuilder<UserItem> builder)
    {
        builder.HasKey(ui => new { ui.RichbetUserId, ui.ItemId });
        builder.ToTable("userItems");
    }
}