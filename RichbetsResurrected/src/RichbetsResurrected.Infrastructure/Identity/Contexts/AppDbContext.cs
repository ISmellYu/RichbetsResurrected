using Ardalis.EFCore.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RichbetsResurrected.Core.DiscordAggregate;
using RichbetsResurrected.Infrastructure.Identity.Models;
using RichbetsResurrected.SharedKernel;

namespace RichbetsResurrected.Infrastructure.Identity.Contexts;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
{
    private readonly IMediator? _mediator;
    
    public DbSet<RichbetAppUser> RichbetAppUsers { get; set; }
    public DbSet<RichbetUser> RichbetUsers { get; set; }

    //public AppDbContext(DbContextOptions options) : base(options)
    //{
    //}

    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator? mediator)
        : base(options)
    {
        _mediator = mediator;
    }
    

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

        // ignore events if no dispatcher provided
        if (_mediator == null) return result;

        // dispatch events only if save was successful
        var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.Events.Any())
            .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.Events.ToArray();
            entity.Events.Clear();
            foreach (var domainEvent in events) await _mediator.Publish(domainEvent).ConfigureAwait(false);
        }

        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}