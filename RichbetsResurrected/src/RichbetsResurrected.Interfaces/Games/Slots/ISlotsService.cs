using RichbetsResurrected.Entities.Games.Slots;

namespace RichbetsResurrected.Interfaces.Games.Slots;

public interface ISlotsService
{
    Task<SlotsSpinResult> SpinAsync(SlotsSpinRequest request, string connectionId);
}