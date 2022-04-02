using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Infrastructure.Identity.Models;

namespace RichbetsResurrected.Infrastructure.Identity.Config.IdentityConfig;

public class AppUserLoginConfig : IEntityTypeConfiguration<AppUserLogin>
{

    public void Configure(EntityTypeBuilder<AppUserLogin> builder)
    {
        builder.ToTable("userLogins");
    }
}