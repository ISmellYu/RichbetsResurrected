using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;

namespace RichbetsResurrected.Identity.Identity.IdentityConfig;

public class AppUserTokenConfig : IEntityTypeConfiguration<AppUserToken>
{

    public void Configure(EntityTypeBuilder<AppUserToken> builder)
    {
        builder.ToTable("userTokens");
    }
}