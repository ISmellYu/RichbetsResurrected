using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;
using RichbetsResurrected.Entities.Games.Slots;
using RichbetsResurrected.Interfaces.Games.Slots;
using SignalRSwaggerGen.Attributes;

namespace RichbetsResurrected.Communication.Slots.Hub;

[SignalRHub("/slotsHub")]
[Authorize]
[SuppressMessage("ReSharper", "AsyncApostle.AsyncMethodNamingHighlighting")]
public class SlotsHub : Hub<ISlotsHub>
{
    private readonly ISlotsService _slotsService;
    public SlotsHub(ISlotsService slotsService)
    {
        _slotsService = slotsService;
    }
    
    [SignalRMethod(summary: "Invokable by clients to start a spin")]
    public async Task<SlotsSpinResult> Spin(int betAmount, int delayAmountToWithdraw)
    {
        var userId = Convert.ToInt32(Context.UserIdentifier);
        var request = new SlotsSpinRequest
        {
            UserId = userId,
            Amount = betAmount,
            DelayAmountToWithdraw = delayAmountToWithdraw
        };
        var result = await _slotsService.SpinAsync(request, Context.ConnectionId);
        return result;
    }
    
    
}