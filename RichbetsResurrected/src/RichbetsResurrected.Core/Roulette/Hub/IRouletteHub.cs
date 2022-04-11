using RichbetsResurrected.Core.Roulette.Entities;

namespace RichbetsResurrected.Core.Roulette.Hub;

public interface IRouletteHub
{
    Task<RouletteInfo> RouletteHelloAsync();
    Task<RouletteJoinResult> JoinRouletteAsync(int amount, RouletteColor color);
    Task TestAsync();
}