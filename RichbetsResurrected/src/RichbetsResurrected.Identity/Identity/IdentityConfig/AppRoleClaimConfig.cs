using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;

namespace RichbetsResurrected.Identity.Identity.IdentityConfig;

public class AppRoleClaimConfig : IEntityTypeConfiguration<AppRoleClaim>
{

    public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
    {
        builder.ToTable("roleClaims");
    }
}