using Autofac;
using Autofac.Core;
using RichbetsResurrected.Interfaces.Games;
using RichbetsResurrected.Interfaces.Games.Crash;
using RichbetsResurrected.Interfaces.Games.Roulette;
using RichbetsResurrected.Interfaces.Games.Slots;
using RichbetsResurrected.Interfaces.Inventory;
using RichbetsResurrected.Interfaces.Shop;
using RichbetsResurrected.Interfaces.Utils;
using RichbetsResurrected.Services.Client;
using RichbetsResurrected.Services.Games.Crash;
using RichbetsResurrected.Services.Games.Roulette;
using RichbetsResurrected.Services.Games.Slots;
using RichbetsResurrected.Services.ScheduledTasks;
using RichbetsResurrected.Services.Shop;
using RichbetsResurrected.Services.Utils;

namespace RichbetsResurrected.Services;

public class DefaultServiceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterScheduledTasks(builder);
        builder.RegisterType<BackgroundTasks>().As<IBackgroundTasks>().SingleInstance();
        RegisterShop(builder);
        RegisterGames(builder);
    }

    private void RegisterGames(ContainerBuilder builder)
    {
        RegisterRoulette(builder);
        RegisterCrash(builder);
        RegisterSlots(builder);
    }

    private void RegisterRoulette(ContainerBuilder builder)
    {
        builder.RegisterType<RouletteGameState>().As<IRouletteGameState>().SingleInstance();
        builder.RegisterType<RouletteService>().As<IRouletteService>().InstancePerLifetimeScope();

        builder.RegisterType<RouletteWorker>().As<IRouletteWorker>()
            .SingleInstance()
            .AutoActivate() // Same instance for everything// Resolve the service before anything else once to create the instance
            .OnActivated(StartGame); // Run a function at service creation
    }

    private void RegisterCrash(ContainerBuilder builder)
    {
        builder.RegisterType<CrashGameState>().As<ICrashGameState>().SingleInstance();
        builder.RegisterType<CrashService>().As<ICrashService>().InstancePerLifetimeScope();

        builder.RegisterType<CrashWorker>().As<ICrashWorker>()
            .SingleInstance() // Same instance for everything
            .AutoActivate() // Resolve the service before anything else once to create the instance
            .OnActivated(StartGame); // Run a function at service creation
    }

    private void RegisterShop(ContainerBuilder builder)
    {
        builder.RegisterType<ShopService>().As<IShopService>().InstancePerLifetimeScope();
        builder.RegisterType<InventoryService>().As<IInventoryService>().InstancePerLifetimeScope();
    }

    private void RegisterSlots(ContainerBuilder builder)
    {
        builder.RegisterType<SlotsService>().As<ISlotsService>().InstancePerLifetimeScope();
    }


    private static void StartGame<T>(IActivatedEventArgs<T> gameArgs) where T : IStartableGame
    {
        _ = Task.Run(() => gameArgs.Instance.StartAsync());
    }

    private static void RegisterScheduledTasks(ContainerBuilder builder)
    {
        builder.RegisterType<ScheduledTasksControl>().As<IScheduledTasksControl>().SingleInstance().AutoActivate();

        builder.RegisterType<DailyResetScheduled>().AsSelf().SingleInstance().AutoActivate().OnActivated(args =>
        {
            _ = Task.Run(() => args.Instance.ExecuteAsync());
        });
    }
}
