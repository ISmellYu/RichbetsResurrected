using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Communication.Crash.Hub;
using RichbetsResurrected.Entities.Crash;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Games;
using RichbetsResurrected.Interfaces.Games.Crash;
using RichbetsResurrected.Utilities.Constants;
using RichbetsResurrected.Utilities.Helpers;

namespace RichbetsResurrected.Services.Games.Crash;

public class CrashService : ICrashService
{
    private BlockingCollection<CrashPlayer> Players { get; set; } = new();
    private List<CrashResult> History { get; set; } = new();
    
    private bool IsRunning { get; set; }
    private bool AllowPlacingBets { get; set; }
    private bool AllowRemovingBets { get; set; }
    private bool Crashed { get; set; }
    
    private decimal Multiplier { get; set; } = 1;
    private decimal TimeLeft { get; set; }

    private readonly IMediator _mediator;
    private readonly IRichbetRepository _repository;
    public CrashService(IRichbetRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }
    
    public async Task StartAsync()
    {
        try
        {
            IsRunning = true;
            while (true)
            {
                ResetCrash();
                await WaitForPlayersAsync();
                var winNumber = CrashHelper.RandomMultiplier();
                

            }
        }
        catch (Exception ex)
        {
            
        }
        IsRunning = false;
    }
    
    private async Task WaitForPlayersAsync()
    {
        TurnOnBetting();
        for (decimal i = CrashConfigs.TimeForUsersToBet; i <= 0; i -= 0.1m)
        {
            TimeLeft = i;
            await Task.Delay(10);
        }
        TurnOffBetting();
        await Task.Delay(100); // Just to make sure every bet is done adding
    }
    
    public CrashInfo GetCrashInfo()
    {
        var crashInfo = new CrashInfo()
        {
            Results = History.TakeLast(10).ToList(), 
            Crashed = Crashed, 
            AllowPlacingBets = AllowPlacingBets,
            AllowRemovingBets = AllowRemovingBets,
            Multiplier = Multiplier, 
            TimeLeft = TimeLeft
        };
        return crashInfo;
    }
    
    private bool CanPlaceBet()
    {
        return AllowPlacingBets && IsRunning;
    }
    
    private void TurnOnBetting()
    {
        AllowPlacingBets = true;
    }
    
    private void TurnOffBetting()
    {
        AllowPlacingBets = false;
    }
    
    private void ClearPlayers()
    {
        while (Players.TryTake(out _))
        {
        }
    }
    
    private void ResetCrash()
    {
        ClearPlayers();
        Crashed = false;
        AllowPlacingBets = true;
        AllowRemovingBets = false;
        Multiplier = 1;
    }
}