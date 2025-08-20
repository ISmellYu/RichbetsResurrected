using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Communication.Roulette.Events;
using RichbetsResurrected.Communication.Roulette.Hub;

namespace RichbetsResurrected.Communication.Roulette.Handlers;

public class EndRouletteNotificationHandler : INotificationHandler<EndRouletteNotification>
{
    private readonly IHubContext<RouletteHub, IRouletteHub> _hubContext;

    public EndRouletteNotificationHandler(IHubContext<RouletteHub, IRouletteHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task Handle(EndRouletteNotification notification, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.EndRoulette(notification.History, notification.Current);
    }
}
