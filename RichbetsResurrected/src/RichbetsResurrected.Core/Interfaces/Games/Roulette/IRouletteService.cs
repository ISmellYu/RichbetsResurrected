using RichbetsResurrected.Core.Roulette.Entities;
using RichbetsResurrected.Core.Roulette.ToView;

namespace RichbetsResurrected.Core.Interfaces.Games.Roulette;

public interface IRouletteService : IStartableGame
{
    void TurnOnBetting();
    void TurnOffBetting();
    Task AddPlayerAsync(RoulettePlayer player);
    bool CheckIfCanBet();
    Task<List<RoulettePlayerToView>> GetPlayersToViewAsync();
}