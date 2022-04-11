using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Core.Roulette.Events;
using RichbetsResurrected.Core.Roulette.Hub;

namespace RichbetsResurrected.Core.Roulette.Handlers;

public class UpdateTimerNotificationHandler : INotificationHandler<UpdateTimerNotification>
{
    private readonly IHubContext<RouletteBaseHub> _hubContext;
    public UpdateTimerNotificationHandler(IHubContext<RouletteBaseHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public Task Handle(UpdateTimerNotification notification, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.SendAsync("UpdateTimer", notification.TimeLeft, cancellationToken: cancellationToken);
    }
}