using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.Roulette;

namespace RichbetsResurrected.Interfaces.Games.Roulette;

public interface IRouletteService : IStartableGame
{
    IRouletteGameState GameState { get; }
    Task<RouletteJoinResult> AddPlayerAsync(RoulettePlayer player);
}