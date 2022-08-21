using RichbetsResurrected.Entities.Games.Slots;

namespace RichbetsResurrected.Interfaces.Utils;

public interface IBackgroundTasks
{
    Task DelaySlotsWithdrawalAsync(SlotsWithdrawResult result, int millisecondsDelay, int userId, string connectionId);
    Task DelayAddingPointsAsync(int userId, int points, int millisecondsDelay);

}