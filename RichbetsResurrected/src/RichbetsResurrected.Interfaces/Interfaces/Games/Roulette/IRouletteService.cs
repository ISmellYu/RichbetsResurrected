using System.Threading.Tasks;
using RichbetsResurrected.Entities.Roulette;

namespace RichbetsResurrected.Interfaces.Interfaces.Games.Roulette;

public interface IRouletteService : IStartableGame
{
    void TurnOnBetting();
    void TurnOffBetting();
    Task<RouletteJoinResult> AddPlayerAsync(RoulettePlayer player);
    bool CheckIfCanBet();
    Task<RouletteInfo> GetRouletteInfoAsync();
}