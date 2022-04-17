using RichbetsResurrected.Entities.Roulette;

namespace RichbetsResurrected.Interfaces.Games.Roulette;

public interface IRouletteService
{
    IRouletteGameState GameState { get; }
    Task<RouletteJoinResult> AddPlayerAsync(RoulettePlayer player);
}