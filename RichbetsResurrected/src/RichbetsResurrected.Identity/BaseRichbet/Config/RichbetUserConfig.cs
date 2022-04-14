using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities;

namespace RichbetsResurrected.Identity.BaseRichbet.Config;

public class RichbetUserConfiguration : IEntityTypeConfiguration<RichbetUser>
{

    public void Configure(EntityTypeBuilder<RichbetUser> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasOne<RichbetAppUser>().WithOne().HasForeignKey<RichbetAppUser>(u => u.RichbetUserId).IsRequired();
        builder.ToTable("richbetUsers");
    }
}