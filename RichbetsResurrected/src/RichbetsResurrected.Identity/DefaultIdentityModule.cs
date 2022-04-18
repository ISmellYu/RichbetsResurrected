using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RichbetsResurrected.Identity.BaseRichbet;
using RichbetsResurrected.Identity.Contexts;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Identity;

namespace RichbetsResurrected.Identity;

public class DefaultIdentityModule : Module
{
    private readonly IConfiguration _configuration;
    public DefaultIdentityModule(IConfiguration configuration)
    {
        _configuration = configuration;

    }
    protected override void Load(ContainerBuilder builder)
    {
        var connectionString = _configuration.GetConnectionString("SqliteConnection");
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connectionString).EnableSensitiveDataLogging();

        builder.RegisterType<AppDbContext>()
            .WithParameter("options", dbContextOptionsBuilder.Options);

        RegisterStores(builder);
        RegisterRepositories(builder);
    }

    private void RegisterStores(ContainerBuilder builder)
    {
    }

    private void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<AccountRepository>().As<IAccountRepository>().InstancePerLifetimeScope();
        builder.RegisterType<RichbetRepository>().As<IRichbetRepository>().InstancePerLifetimeScope();
    }
}