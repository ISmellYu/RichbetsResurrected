using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Core.Roulette.Events;
using RichbetsResurrected.Core.Roulette.Hub;

namespace RichbetsResurrected.Core.Roulette.Handlers;

public class EndRouletteNotificationHandler : INotificationHandler<EndRouletteNotification>
{
    private readonly IHubContext<RouletteBaseHub> _hubContext;
    public EndRouletteNotificationHandler(IHubContext<RouletteBaseHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public Task Handle(EndRouletteNotification notification, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.SendAsync("EndRoulette", notification, cancellationToken: cancellationToken);
    }
}