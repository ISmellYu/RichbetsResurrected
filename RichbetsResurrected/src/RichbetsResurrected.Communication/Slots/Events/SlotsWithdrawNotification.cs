using MediatR;
using RichbetsResurrected.Entities.Games.Slots;

namespace RichbetsResurrected.Communication.Slots.Events;

public class SlotsWithdrawNotification : INotification
{
    public SlotsWithdrawNotification(SlotsWithdrawResult result, string connectionId)
    {
        Result = result;
        ConnectionId = connectionId;

    }
    
    public SlotsWithdrawResult Result { get; set; }
    public string ConnectionId { get; set; }
}