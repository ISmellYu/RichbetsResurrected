using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.Identity.Models;

namespace RichbetsResurrected.Identity.Config.IdentityConfig;

public class AppUserRoleConfig : IEntityTypeConfiguration<AppUserRole>
{

    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.ToTable("userRoles");
    }
}