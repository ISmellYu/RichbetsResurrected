using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Infrastructure.Data.Identity.Models;

namespace RichbetsResurrected.Infrastructure.Data.Config.IdentityConfig;

public class AppUserClaimConfig : IEntityTypeConfiguration<AppUserClaim>
{

    public void Configure(EntityTypeBuilder<AppUserClaim> builder)
    {
        builder.ToTable("userClaims");
    }
}