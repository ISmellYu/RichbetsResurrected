using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;

namespace RichbetsResurrected.Identity.Identity.IdentityConfig;

public class AppUserRoleConfig : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.ToTable("userRoles");
    }
}
