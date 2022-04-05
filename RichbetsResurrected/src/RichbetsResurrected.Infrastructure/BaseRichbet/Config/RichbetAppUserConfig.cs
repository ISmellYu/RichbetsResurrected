using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Core.DiscordAggregate;

namespace RichbetsResurrected.Infrastructure.BaseRichbet.Config;

public class RichbetAppUserConfiguration : IEntityTypeConfiguration<RichbetAppUser>
{

    public void Configure(EntityTypeBuilder<RichbetAppUser> builder)
    {
        builder.HasKey(u => new {u.AppUserId, u.RichbetUserId, u.DiscordUserId});
        builder.ToTable("richbetAppUser");
    }
}