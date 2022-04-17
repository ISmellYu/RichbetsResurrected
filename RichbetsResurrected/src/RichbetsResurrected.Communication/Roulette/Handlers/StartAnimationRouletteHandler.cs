using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Communication.Roulette.Events;
using RichbetsResurrected.Communication.Roulette.Hub;

namespace RichbetsResurrected.Communication.Roulette.Handlers;

public class StartAnimationRouletteHandler : INotificationHandler<StartAnimationNotification>
{
    private readonly IHubContext<RouletteHub, IRouletteHub> _hubContext;
    public StartAnimationRouletteHandler(IHubContext<RouletteHub, IRouletteHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public Task Handle(StartAnimationNotification notification, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.StartAnimation(notification.StopAt);
    }
}