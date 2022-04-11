using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Core.Roulette.Events;
using RichbetsResurrected.Core.Roulette.Hub;

namespace RichbetsResurrected.Core.Roulette.Handlers;

public class StartAnimationRouletteHandler : INotificationHandler<StartAnimationNotification>
{
    private readonly IHubContext<RouletteBaseHub> _hubContext;
    public StartAnimationRouletteHandler(IHubContext<RouletteBaseHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public Task Handle(StartAnimationNotification notification, CancellationToken cancellationToken)
    {
        // _rouletteHub.TestAsync();
        return _hubContext.Clients.All.SendAsync("StartAnimation", notification.StopAt, cancellationToken: cancellationToken);
    }
}