using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Core.Interfaces.Games.Roulette;
using RichbetsResurrected.Core.Roulette.Events;
using RichbetsResurrected.Core.Roulette.Hub;

namespace RichbetsResurrected.Core.Roulette.Handlers;

public class NewPlayerJoinedRouletteNotificationHandler : INotificationHandler<NewPlayerJoinedNotification>
{
    private readonly IHubContext<RouletteBaseHub> _hubContext;
    public NewPlayerJoinedRouletteNotificationHandler(IHubContext<RouletteBaseHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public Task Handle(NewPlayerJoinedNotification notification, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.SendAsync("NewPlayerJoined", notification.Player, cancellationToken: cancellationToken);
    }
}