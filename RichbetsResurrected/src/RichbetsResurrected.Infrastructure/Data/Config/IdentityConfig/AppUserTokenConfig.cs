using Autofac.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Infrastructure.Data.Identity.Models;

namespace RichbetsResurrected.Infrastructure.Data.Config.IdentityConfig;

public class AppUserTokenConfig : IEntityTypeConfiguration<AppUserToken>
{

    public void Configure(EntityTypeBuilder<AppUserToken> builder)
    {
        builder.ToTable("userTokens");
    }
}