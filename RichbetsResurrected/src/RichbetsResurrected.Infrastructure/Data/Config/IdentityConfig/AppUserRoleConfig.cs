﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Infrastructure.Data.Identity.Models;

namespace RichbetsResurrected.Infrastructure.Data.Config.IdentityConfig;

public class AppUserRoleConfig : IEntityTypeConfiguration<AppUserRole>
{

    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.ToTable("userRoles");
    }
}