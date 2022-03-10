using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Infrastructure.Data.Identity.Models;

namespace RichbetsResurrected.Infrastructure.Data.Config.IdentityConfig;

public class AppRoleClaimConfig : IEntityTypeConfiguration<AppRoleClaim>
{

    public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
    {
        builder.ToTable("roleClaims");
    }
}