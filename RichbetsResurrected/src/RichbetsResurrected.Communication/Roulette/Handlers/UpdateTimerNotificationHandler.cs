using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Communication.Roulette.Events;
using RichbetsResurrected.Communication.Roulette.Hub;

namespace RichbetsResurrected.Communication.Roulette.Handlers;

public class UpdateTimerNotificationHandler : INotificationHandler<UpdateTimerNotification>
{
    private readonly IHubContext<RouletteHub, IRouletteHub> _hubContext;
    public UpdateTimerNotificationHandler(IHubContext<RouletteHub, IRouletteHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public Task Handle(UpdateTimerNotification notification, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.UpdateTimer(notification.TimeLeft);
    }
}