using RichbetsResurrected.Core.Roulette.Entities;
using RichbetsResurrected.Core.Roulette.ToView;

namespace RichbetsResurrected.Core.Interfaces.Games.Roulette;

public interface IRouletteService : IStartableGame
{
    void TurnOnBetting();
    void TurnOffBetting();
    Task<RouletteJoinResult> AddPlayerAsync(RoulettePlayer player);
    bool CheckIfCanBet();
    List<RoulettePlayerToView> GetPlayersToViewAsync();
    Task<RouletteInfo> GetRouletteInfoAsync();
}