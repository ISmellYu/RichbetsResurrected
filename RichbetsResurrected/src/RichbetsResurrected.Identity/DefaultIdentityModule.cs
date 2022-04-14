using Autofac;
using RichbetsResurrected.Identity.BaseRichbet;
using RichbetsResurrected.Identity.BaseRichbet.Stores;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.DAL.Stores;
using RichbetsResurrected.Interfaces.Identity;

namespace RichbetsResurrected.Identity;

public class DefaultIdentityModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterStores(builder);
        RegisterRepositories(builder);
    }

    private void RegisterStores(ContainerBuilder builder)
    {
        builder.RegisterType<RichbetStore>().As<IRichbetStore>().InstancePerLifetimeScope();
    }

    private void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<AccountRepository>().As<IAccountRepository>().InstancePerLifetimeScope();
        builder.RegisterType<RichbetRepository>().As<IRichbetRepository>().InstancePerLifetimeScope();
    }
}