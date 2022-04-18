using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;
using RichbetsResurrected.Entities.Identity.Models;

namespace RichbetsResurrected.Identity.Identity.IdentityConfig;

public class AppUserConfig : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasOne<RichbetAppUser>().WithOne().HasForeignKey<RichbetAppUser>(x => x.AppUserId).IsRequired();
        builder.ToTable("users");
    }
}