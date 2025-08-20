using MediatR;

namespace RichbetsResurrected.Communication.Roulette.Events;

public class UpdateTimerNotification : INotification
{
    public UpdateTimerNotification(int timeLeft)
    {
        TimeLeft = timeLeft;
    }

    public int TimeLeft { get; set; }
}
