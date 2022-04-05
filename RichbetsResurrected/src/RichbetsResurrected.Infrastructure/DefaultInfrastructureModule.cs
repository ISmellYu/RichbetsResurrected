using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using RichbetsResurrected.Core.Interfaces;
using RichbetsResurrected.Core.Interfaces.Stores;
using RichbetsResurrected.Core.ProjectAggregate;
using RichbetsResurrected.Infrastructure.BaseRichbet;
using RichbetsResurrected.Infrastructure.BaseRichbet.Stores;
using RichbetsResurrected.Infrastructure.Data;
using RichbetsResurrected.Infrastructure.Identity;
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
            Assembly.GetAssembly(typeof(Project)); // TODO: Replace "Project" with any type from your Core project
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
            typeof(IRequestHandler<,>), typeof(IRequestExceptionHandler<,,>), typeof(IRequestExceptionAction<,>), typeof(INotificationHandler<>)
        };

        foreach (var mediatrOpenType in mediatrOpenTypes)
            builder
                .RegisterAssemblyTypes(_assemblies.ToArray())
                .AsClosedTypesOf(mediatrOpenType)
                .AsImplementedInterfaces();

        builder.RegisterType<EmailSender>().As<IEmailSender>()
            .InstancePerLifetimeScope();

        builder.RegisterType<AccountRepository>().AsSelf().InstancePerLifetimeScope();
        builder.RegisterType<RichbetStore>().As<IRichbetStore>().InstancePerLifetimeScope();
        builder.RegisterType<RichbetRepository>().As<IRichbetRepository>().InstancePerLifetimeScope();

    }

    private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
    {
        // TODO: Add development only services
    }

    private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
    {
        // TODO: Add production only services
    }
}