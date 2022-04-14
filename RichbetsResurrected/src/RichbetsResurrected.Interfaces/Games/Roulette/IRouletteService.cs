using RichbetsResurrected.Entities.Roulette;

namespace RichbetsResurrected.Interfaces.Games.Roulette;

public interface IRouletteService : IStartableGame
{
    void TurnOnBetting();
    void TurnOffBetting();
    Task<RouletteJoinResult> AddPlayerAsync(RoulettePlayer player);
    bool CheckIfCanBet();
    RouletteInfo GetRouletteInfoAsync();
}