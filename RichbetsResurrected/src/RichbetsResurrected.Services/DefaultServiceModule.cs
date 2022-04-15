using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using RichbetsResurrected.Interfaces.Games;
using RichbetsResurrected.Interfaces.Games.Roulette;
using RichbetsResurrected.Services.Games.Roulette;

namespace RichbetsResurrected.Services;

public class DefaultServiceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterGames(builder);
    }

    private void RegisterGames(ContainerBuilder builder)
    {
        RegisterRoulette(builder);
    }

    private void RegisterRoulette(ContainerBuilder builder)
    {
        builder.RegisterType<RouletteGameState>().As<IRouletteGameState>().SingleInstance();

        builder.RegisterType<RouletteService>().As<IRouletteService>()
        .InstancePerLifetimeScope()
        .AutoActivate() // Same instance for everything// Resolve the service before anything else once to create the instance
        .OnActivated(StartGame); // Run a function at service creation
    }
    
    
    private static void StartGame<T>(IActivatedEventArgs<T> gameArgs) where T : IStartableGame
    {
        _ = Task.Run(() => gameArgs.Instance.StartAsync());
    }
}