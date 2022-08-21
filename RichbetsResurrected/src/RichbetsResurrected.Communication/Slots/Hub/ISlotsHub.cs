using System.Diagnostics.CodeAnalysis;
using RichbetsResurrected.Entities.Games.Slots;
using SignalRSwaggerGen.Attributes;

namespace RichbetsResurrected.Communication.Slots.Hub;
[SuppressMessage("ReSharper", "AsyncApostle.AsyncMethodNamingHighlighting")]
public interface ISlotsHub
{
    [SignalRMethod(summary: "Sends to specified user a withdraw end message that contains if he wins, amount won and multiplier")]
    Task WithdrawEnd(SlotsWithdrawResult withdrawResult);
}