using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.Identity.Models;

namespace RichbetsResurrected.Identity.Identity.IdentityConfig;

public class AppRoleConfig : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable("roles");
    }
}