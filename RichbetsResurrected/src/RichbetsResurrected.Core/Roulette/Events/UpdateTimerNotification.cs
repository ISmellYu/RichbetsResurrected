using MediatR;

namespace RichbetsResurrected.Core.Roulette.Events;

public class UpdateTimerNotification : INotification
{
    public UpdateTimerNotification(int timeLeft)
    {
        TimeLeft = timeLeft;
    }
    public int TimeLeft { get; set; }
}