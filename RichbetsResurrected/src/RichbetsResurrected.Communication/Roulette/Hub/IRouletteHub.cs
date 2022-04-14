using System.Collections.Generic;
using System.Threading.Tasks;
using RichbetsResurrected.Entities.Roulette;

namespace RichbetsResurrected.Communication.Roulette.Hub;

public interface IRouletteHub
{
    Task UpdateTimer(int timeLeft);
    Task EndRoulette(List<RouletteResult> history, RouletteResult current);
    Task PlayerJoined(RoulettePlayer player);
    Task StartAnimation(double stopAt);
}