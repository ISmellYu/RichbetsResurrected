using RichbetsResurrected.Entities.Games.Slots;

namespace RichbetsResurrected.Interfaces.Utils;

public interface IBackgroundTasks
{
    Task DelayWithdrawalAsync(SlotsWithdrawResult result, int delay, int userId, string connectionId);
}