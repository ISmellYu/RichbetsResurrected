using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Communication.Roulette.Events;
using RichbetsResurrected.Communication.Roulette.Hub;

namespace RichbetsResurrected.Communication.Roulette.Handlers;

public class NewPlayerJoinedRouletteNotificationHandler : INotificationHandler<NewPlayerJoinedNotification>
{
    private readonly IHubContext<RouletteHub, IRouletteHub> _hubContext;

    public NewPlayerJoinedRouletteNotificationHandler(IHubContext<RouletteHub, IRouletteHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task Handle(NewPlayerJoinedNotification notification, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.PlayerJoined(notification.Player);
    }
}
