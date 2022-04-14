using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Communication.Crash.Hub;
using RichbetsResurrected.Entities.Crash;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Games;

namespace RichbetsResurrected.Services.Games.Crash;

public class CrashService : IStartableGame
{
    private BlockingCollection<CrashPlayer> Players { get; set; } = new();
    private List<CrashResult> History { get; set; } = new();
    
    private bool IsRunning { get; set; }
    private bool AllowPlacingBets { get; set; }
    private bool AllowRemovingBets { get; set; }
    private bool Crashed { get; set; }
    
    private decimal Multiplier { get; set; } = 1;

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
                
            }
        }
        catch (Exception ex)
        {
            
        }
        IsRunning = false;
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