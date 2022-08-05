using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Communication.Slots.Events;
using RichbetsResurrected.Communication.Slots.Hub;

namespace RichbetsResurrected.Communication.Slots.Handlers;

public class SlotsWithdrawHandler : INotificationHandler<SlotsWithdrawNotification>
{
    private readonly IHubContext<SlotsHub, ISlotsHub> _hubContext;
    public SlotsWithdrawHandler(IHubContext<SlotsHub, ISlotsHub> hubContext)
    {
        _hubContext = hubContext;

    }
    public Task Handle(SlotsWithdrawNotification notification, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.Client(notification.ConnectionId).WithdrawEnd(notification.Result);
    }
}