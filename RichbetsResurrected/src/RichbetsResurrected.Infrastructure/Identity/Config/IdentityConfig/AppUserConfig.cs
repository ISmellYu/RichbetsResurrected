using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Core.DiscordAggregate;
using RichbetsResurrected.Infrastructure.Identity.Models;

namespace RichbetsResurrected.Infrastructure.Identity.Config.IdentityConfig;

public class AppUserConfig : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasOne<RichbetAppUser>().WithOne().HasForeignKey<RichbetAppUser>(x => x.AppUserId).IsRequired();
        builder.ToTable("users");
    }
}