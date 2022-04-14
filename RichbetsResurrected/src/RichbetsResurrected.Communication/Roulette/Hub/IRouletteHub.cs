using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RichbetsResurrected.Entities.Roulette;
using SignalRSwaggerGen.Attributes;

namespace RichbetsResurrected.Communication.Roulette.Hub;

[SignalRHub("/rouletteHub")]
[SuppressMessage("ReSharper", "AsyncApostle.AsyncMethodNamingHighlighting")]
public interface IRouletteHub
{
    // Dont end functions with Async suffix
    [SignalRMethod(summary: "Listen for new timer update")]
    Task UpdateTimer(int timeLeft);
    [SignalRMethod(summary: "Listen for end roulette")]
    Task EndRoulette(List<RouletteResult> history, RouletteResult current);
    [SignalRMethod(summary: "Listen for new player")]
    Task PlayerJoined(RoulettePlayer player);
    [SignalRMethod(summary: "Listen for start animation")]
    Task StartAnimation(double stopAt);
}