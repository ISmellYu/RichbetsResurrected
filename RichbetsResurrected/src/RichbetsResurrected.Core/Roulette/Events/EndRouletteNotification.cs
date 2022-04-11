using MediatR;
using RichbetsResurrected.Core.Roulette.Entities;

namespace RichbetsResurrected.Core.Roulette.Events;

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