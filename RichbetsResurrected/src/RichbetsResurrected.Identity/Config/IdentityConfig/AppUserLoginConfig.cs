using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.Identity.Models;

namespace RichbetsResurrected.Identity.Config.IdentityConfig;

public class AppUserLoginConfig : IEntityTypeConfiguration<AppUserLogin>
{

    public void Configure(EntityTypeBuilder<AppUserLogin> builder)
    {
        builder.ToTable("userLogins");
    }
}