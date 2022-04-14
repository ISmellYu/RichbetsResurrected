using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.Identity.Models;

namespace RichbetsResurrected.Identity.Config.IdentityConfig;

public class AppUserTokenConfig : IEntityTypeConfiguration<AppUserToken>
{

    public void Configure(EntityTypeBuilder<AppUserToken> builder)
    {
        builder.ToTable("userTokens");
    }
}