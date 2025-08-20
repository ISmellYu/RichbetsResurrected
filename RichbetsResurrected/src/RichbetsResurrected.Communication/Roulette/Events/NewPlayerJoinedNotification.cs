using MediatR;
using RichbetsResurrected.Entities.Games.Roulette;

namespace RichbetsResurrected.Communication.Roulette.Events;

public class NewPlayerJoinedNotification : INotification
{
    public NewPlayerJoinedNotification(RoulettePlayer player)
    {
        Player = player;
    }

    public RoulettePlayer Player { get; set; }
}
