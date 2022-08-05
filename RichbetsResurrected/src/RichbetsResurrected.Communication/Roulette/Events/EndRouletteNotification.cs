using MediatR;
using RichbetsResurrected.Entities.Games.Roulette;

namespace RichbetsResurrected.Communication.Roulette.Events;

public class EndRouletteNotification : INotification
{
    public EndRouletteNotification(List<RouletteResult> history, RouletteResult current)
    {
        History = history;
        Current = current;
    }
    public List<RouletteResult> History { get; set; }
    public RouletteResult Current { get; set; }
}