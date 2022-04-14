using MediatR;

namespace RichbetsResurrected.Communication.Roulette.Events;

public class StartAnimationNotification : INotification
{
    public StartAnimationNotification(double stopAt)
    {
        StopAt = stopAt;
    }
    public double StopAt { get; set; }
}