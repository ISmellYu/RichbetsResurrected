using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;

namespace RichbetsResurrected.Identity.Identity.IdentityConfig;

public class AppUserLoginConfig : IEntityTypeConfiguration<AppUserLogin>
{

    public void Configure(EntityTypeBuilder<AppUserLogin> builder)
    {
        builder.ToTable("userLogins");
    }
}