using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.Identity.Models;

namespace RichbetsResurrected.Identity.Config.IdentityConfig;

public class AppRoleClaimConfig : IEntityTypeConfiguration<AppRoleClaim>
{

    public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
    {
        builder.ToTable("roleClaims");
    }
}