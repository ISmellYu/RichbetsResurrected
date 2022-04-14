using System.Threading;
using System.Threading.Tasks;
using Ardalis.EFCore.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RichbetsResurrected.Entities.DatabaseEntities;
using RichbetsResurrected.Entities.Identity.Models;

namespace RichbetsResurrected.Identity.Contexts;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
{

    //public AppDbContext(DbContextOptions options) : base(options)
    //{
    //}

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<RichbetAppUser> RichbetAppUsers { get; set; }
    public DbSet<RichbetUser> RichbetUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();

        // alternately this is built-in to EF Core 2.2
        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}