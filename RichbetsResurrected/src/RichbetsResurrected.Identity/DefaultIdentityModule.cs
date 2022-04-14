using Autofac;
using RichbetsResurrected.Identity.BaseRichbet;
using RichbetsResurrected.Identity.BaseRichbet.Stores;
using RichbetsResurrected.Interfaces.Identity;
using RichbetsResurrected.Interfaces.Interfaces;
using RichbetsResurrected.Interfaces.Interfaces.Stores;

namespace RichbetsResurrected.Identity;

public class DefaultIdentityModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RichbetStore>().As<IRichbetStore>().InstancePerLifetimeScope();


        builder.RegisterType<AccountRepository>().As<IAccountRepository>().InstancePerLifetimeScope();
        builder.RegisterType<RichbetRepository>().As<IRichbetRepository>().InstancePerLifetimeScope();
    }
}