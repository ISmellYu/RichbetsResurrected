using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using RichbetsResurrected.Communication.Client.Hub;
using RichbetsResurrected.Communication.Crash.Hub;
using RichbetsResurrected.Communication.Roulette.Hub;
using RichbetsResurrected.Communication.Slots.Hub;
using Module = Autofac.Module;

namespace RichbetsResurrected.Communication;

public class DefaultCommunicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<Mediator>()
            .As<IMediator>()
            .InstancePerLifetimeScope();

        builder.Register<ServiceFactory>(context =>
        {
            var c = context.Resolve<IComponentContext>();
            return t => c.Resolve(t);
        });

        var mediatrOpenTypes = new[]
        {
            typeof(IRequestHandler<,>), typeof(IRequestExceptionHandler<,,>), typeof(IRequestExceptionAction<,>),
            typeof(INotificationHandler<>), typeof(IStreamRequestHandler<,>)
        };

        foreach (var mediatrOpenType in mediatrOpenTypes)
        {
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(mediatrOpenType)
                .AsImplementedInterfaces();
        }

        //builder.RegisterHubs(Assembly.GetExecutingAssembly());
        builder.RegisterType<RouletteHub>().ExternallyOwned();
        builder.RegisterType<CrashHub>().ExternallyOwned();
        builder.RegisterType<ClientHub>().ExternallyOwned();
        builder.RegisterType<SlotsHub>().ExternallyOwned();
    }
}
