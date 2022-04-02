using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Infrastructure.Identity.Models;

namespace RichbetsResurrected.Infrastructure.Identity.Config.IdentityConfig;

public class AppUserTokenConfig : IEntityTypeConfiguration<AppUserToken>
{

    public void Configure(EntityTypeBuilder<AppUserToken> builder)
    {
        builder.ToTable("userTokens");
    }
}