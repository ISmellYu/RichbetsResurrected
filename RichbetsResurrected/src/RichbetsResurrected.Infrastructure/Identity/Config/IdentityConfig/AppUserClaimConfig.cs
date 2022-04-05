using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Core.DiscordAggregate;
using RichbetsResurrected.Infrastructure.Identity.Models;

namespace RichbetsResurrected.Infrastructure.Identity.Config.IdentityConfig;

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