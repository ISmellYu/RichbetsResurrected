using MediatR;

namespace RichbetsResurrected.Core.Roulette.Events;

public class StartAnimationNotification : INotification
{
    public StartAnimationNotification(double stopAt)
    {
        StopAt = stopAt;
    }
    public double StopAt { get; set; }
}