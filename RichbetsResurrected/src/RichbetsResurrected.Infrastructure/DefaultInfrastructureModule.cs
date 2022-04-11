using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Integration.SignalR;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNet.SignalR;
using RichbetsResurrected.Core.DiscordAggregate;
using RichbetsResurrected.Core.Interfaces;
using RichbetsResurrected.Core.Interfaces.Games;
using RichbetsResurrected.Core.Interfaces.Games.Roulette;
using RichbetsResurrected.Core.Interfaces.Stores;
using RichbetsResurrected.Infrastructure.BaseRichbet;
using RichbetsResurrected.Infrastructure.BaseRichbet.Stores;
using RichbetsResurrected.Infrastructure.Data;
using RichbetsResurrected.Infrastructure.Games.Roulette;
using RichbetsResurrected.Infrastructure.Identity;
using RichbetsResurrected.Infrastructure.Identity.Interfaces;
using RichbetsResurrected.SharedKernel.Interfaces;
using Module = Autofac.Module;

namespace RichbetsResurrected.Infrastructure;

public class DefaultInfrastructureModule : Module
{
    private readonly List<Assembly> _assemblies = new();
    private readonly bool _isDevelopment;

    public DefaultInfrastructureModule(bool isDevelopment, Assembly? callingAssembly = null)
    {
        _isDevelopment = isDevelopment;
        var coreAssembly =
            Assembly.GetAssembly(typeof(RichbetUser));
        var infrastructureAssembly = Assembly.GetAssembly(typeof(StartupSetup));
        if (coreAssembly != null) _assemblies.Add(coreAssembly);

        if (infrastructureAssembly != null) _assemblies.Add(infrastructureAssembly);

        if (callingAssembly != null) _assemblies.Add(callingAssembly);
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (_isDevelopment) RegisterDevelopmentOnlyDependencies(builder);
        else RegisterProductionOnlyDependencies(builder);

        RegisterCommonDependencies(builder);
    }

    private void RegisterCommonDependencies(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(EfRepository<>))
            .As(typeof(IRepository<>))
            .As(typeof(IReadRepository<>))
            .InstancePerLifetimeScope();

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
            typeof(IRequestHandler<,>), typeof(IRequestExceptionHandler<,,>), typeof(IRequestExceptionAction<,>), typeof(INotificationHandler<>),
            typeof(IStreamRequestHandler<,>)
        };

        foreach (var mediatrOpenType in mediatrOpenTypes)
            builder
                .RegisterAssemblyTypes(_assemblies.ToArray())
                .AsClosedTypesOf(mediatrOpenType)
                .AsImplementedInterfaces();
        

        
        RegisterStores(builder);
        RegisterRepositories(builder);
        RegisterHubs(builder);
        RegisterGames(builder);
        
    }

    private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
    {
        // TODO: Add development only services
    }

    private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
    {
        // TODO: Add production only services
    }

    private void RegisterGames(ContainerBuilder builder)
    {
        builder.RegisterType<RouletteService>().As<IRouletteService>()
            .SingleInstance()   // Same instance for everything
            .AutoActivate() // Resolve the service before anything else once to create the instance
            .OnActivated(StartGame);  // Run a function at service creation
    }
    private static void StartGame<T>(IActivatedEventArgs<T> gameArgs) where T: IStartableGame
    {
        _ = Task.Run((() => gameArgs.Instance.StartAsync()));
    }

    private void RegisterStores(ContainerBuilder builder)
    {
        builder.RegisterType<RichbetStore>().As<IRichbetStore>().InstancePerLifetimeScope();
    }
    
    private void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<AccountRepository>().AsSelf().InstancePerLifetimeScope();
        builder.RegisterType<RichbetRepository>().As<IRichbetRepository>().InstancePerLifetimeScope();
    }

    private void RegisterHubs(ContainerBuilder builder)
    {
        foreach (var assembly in _assemblies)
        {
            builder.RegisterHubs(assembly);
        }
    }
}