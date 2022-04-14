using Autofac;
using Autofac.Core;
using RichbetsResurrected.Interfaces.Interfaces.Games;
using RichbetsResurrected.Interfaces.Interfaces.Games.Roulette;
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
        builder.RegisterType<RouletteService>().As<IRouletteService>()
            .SingleInstance() // Same instance for everything
            .AutoActivate() // Resolve the service before anything else once to create the instance
            .OnActivated(StartGame); // Run a function at service creation
    }
    private static void StartGame<T>(IActivatedEventArgs<T> gameArgs) where T : IStartableGame
    {
        _ = Task.Run(() => gameArgs.Instance.StartAsync());
    }
}