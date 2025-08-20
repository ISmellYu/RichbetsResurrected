using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;
using RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;

namespace RichbetsResurrected.Identity.Identity.IdentityConfig;

public class AppUserClaimConfig : IEntityTypeConfiguration<AppUserClaim>
{
    public void Configure(EntityTypeBuilder<AppUserClaim> builder)
    {
        builder
            .HasOne<RichbetAppUser>()
            .WithOne()
            .HasForeignKey<RichbetAppUser>(x => x.DiscordUserId)
            .HasPrincipalKey<AppUserClaim>(u => u.ClaimValue);
        builder.ToTable("userClaims");
    }
}
