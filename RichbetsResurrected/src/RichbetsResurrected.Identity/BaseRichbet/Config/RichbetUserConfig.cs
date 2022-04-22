using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

namespace RichbetsResurrected.Identity.BaseRichbet.Config;

public class RichbetUserConfiguration : IEntityTypeConfiguration<RichbetUser>
{

    public void Configure(EntityTypeBuilder<RichbetUser> builder)
    {
        builder.HasKey(u => u.AppUserId);
        builder.ToTable("richbetUsers");
    }
}