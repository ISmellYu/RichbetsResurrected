using MediatR;
using RichbetsResurrected.Core.Roulette.Entities;

namespace RichbetsResurrected.Core.Roulette.Events;

public class NewPlayerJoinedNotification : INotification
{
    public NewPlayerJoinedNotification(RoulettePlayer player)
    {
        Player = player;
    }
    public RoulettePlayer Player { get; set; }
}