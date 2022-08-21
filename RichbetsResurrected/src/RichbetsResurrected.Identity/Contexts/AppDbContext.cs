using Ardalis.EFCore.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RichbetsResurrected.Entities.DatabaseEntities;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;
using RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Entities.DatabaseEntities.Statistics;

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
    public DbSet<ActiveItem> ActiveItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
    public DbSet<ConsumableItem> ConsumableItems { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<UserItem> UserItems { get; set; }
    public DbSet<Statistic> Statistics { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
        
        modelBuilder.Entity<Item>().HasOne<SubCategory>(e => e.SubCategory)
            .WithMany(e => e.Items)
            .HasForeignKey(e => e.SubCategoryId).IsRequired();
        
        
        modelBuilder.Entity<Item>().HasOne<ItemType>(e => e.ItemType).WithOne(e => e.Item)
            .HasForeignKey<ItemType>(e => e.ItemId).IsRequired();

        modelBuilder.Entity<Item>().HasOne<ConsumableItem>(e => e.ConsumableItem)
            .WithOne(e => e.Item);

        modelBuilder.Entity<Item>().HasOne<Discount>(e => e.Discount)
            .WithOne(e => e.Item);

        modelBuilder.Entity<AppUser>().HasOne<RichbetUser>(e => e.RichbetUser)
            .WithOne(e => e.AppUser).IsRequired();
        
        
        modelBuilder.Entity<ActiveItem>().HasOne<RichbetUser>(e => e.RichetUser)
            .WithMany(e => e.ActiveItems).HasForeignKey(e => e.RichetUserId).IsRequired();
        
        modelBuilder.Entity<ActiveItem>().HasOne<Item>(e => e.Item)
            .WithMany(e => e.ActivatedItems).HasForeignKey(e => e.ItemId).IsRequired();
        
        modelBuilder.Entity<UserItem>().HasOne<RichbetUser>(u => u.RichbetUser)
            .WithMany(e => e.UserItems).HasForeignKey(e => e.RichbetUserId).IsRequired();
        
        modelBuilder.Entity<UserItem>().HasOne<Item>(u => u.Item)
            .WithMany(u => u.UserItems).HasForeignKey(e => e.ItemId).IsRequired();
        
        modelBuilder.Entity<SubCategory>().HasOne<Category>(u => u.Category)
            .WithMany(u => u.SubCategories).HasForeignKey(e => e.CategoryId).IsRequired();

        modelBuilder.Entity<Statistic>().HasOne<RichbetUser>(u => u.RichetUser)
            .WithOne(e => e.Statistics).IsRequired();

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