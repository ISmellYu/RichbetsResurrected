using System.Collections.Concurrent;
using MediatR;
using Microsoft.Extensions.Hosting;
using RichbetsResurrected.Core.Interfaces;
using RichbetsResurrected.Core.Interfaces.Games;
using RichbetsResurrected.Core.Interfaces.Games.Roulette;
using RichbetsResurrected.Core.Roulette.Entities;
using RichbetsResurrected.Core.Roulette.ToView;
using RichbetsResurrected.Infrastructure.BaseRichbet;

namespace RichbetsResurrected.Infrastructure.Games.Roulette;

public class RouletteService : IRouletteService
{
    private BlockingCollection<RoulettePlayer> Players { get; set; } = new();
    private List<RouletteResult> History { get; set; } = new();
    private bool IsRunning { get; set; } = false;
    private bool AllowBetting { get; set; } = false;
    private bool IsSpinning { get; set; } = false;
    
    
    private readonly IRichbetRepository _repository;
    private readonly IMediator _mediator;

    public RouletteService(IRichbetRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }
    
    public void TurnOnBetting()
    {
        AllowBetting = true;
    }
    
    public void TurnOffBetting()
    {
        AllowBetting = false;
    }
    
    public async Task AddPlayerAsync(RoulettePlayer player)
    {
        Players.Add(player);
    }
    
    public bool CheckIfCanBet()
    {
        return AllowBetting && IsRunning && !IsSpinning;
    }
    public async Task StartAsync()
    {
        try
        {
            IsRunning = true;
            while (true)
            {
                ResetGame();
                await WaitForPlayersAsync();
                var winNumber = GetRandomWinNumber();
                await SpinAsync(winNumber);
                var winColor = RouletteConstants.GetRouletteColorForNumber(winNumber);
                var result = await AwardWinnersAsync(winNumber, winColor);
                AddToHistory(result);
                // TODO: Send updates to players
            }
        }
        catch (Exception e)
        {
            IsRunning = false;
        }
    }

    private async Task WaitForPlayersAsync()
    {
        TurnOnBetting();
        await Task.Delay(15000);
        TurnOffBetting();
        await Task.Delay(1000); // Just to make sure every bet is done adding
    }

    private async Task SpinAsync(int winNumber)
    {
        IsSpinning = true;
        var segment = RouletteConstants.GetSegmentForNumber(winNumber);
        var stopAt = RouletteConstants.GetRandomAngleForSegment(segment, RouletteConstants.TotalSegments);
        await StartAnimationForClientsAsync(stopAt);
        await Task.Delay(RouletteConstants.SpinDuration);
        IsSpinning = false;
    }

    private async Task StartAnimationForClientsAsync(double stopAt)
    {
        // TODO: Send start spin animation event to clients
    }

    private async Task SendJoinPlayerToClientsAsync(RoulettePlayer player)
    {
        // TODO: Send join player event to clients
    }
    private async Task SendWinNotificationToClientsAsync(RouletteResult result)
    {
        // TODO: Send win notification event to clients
    }

    private async Task SendUpdateTimerToClientsAsync(int timeLeft)
    {
        // TODO: Send update timer event to clients
    }

    private int GetRandomWinNumber()
    {
        return Random.Shared.Next(0, RouletteConstants.TotalSegments - 1);
    }
    
    private void AddToHistory(RouletteResult result)
    {
        History.Add(result);
    }

    private async Task<RouletteResult> AwardWinnersAsync(int number, RouletteColor winColor)
    {
        var winners = Players.Where(p => p.Color == winColor).ToList();
        var losers = Players.Where(p => p.Color != winColor).ToList();
        
        var result = new RouletteResult(number, winColor, winners.ToList(), losers.ToList());
        // TODO: Send result to winners
        await SendWinNotificationToClientsAsync(result);
        switch (winColor)
        {
            case RouletteColor.Black or RouletteColor.Red:
                foreach (var winner in winners)
                {
                    await _repository.AddPointsToUserAsync(winner.IdentityUserId, winner.Amount * 2);
                }
                break;
            case RouletteColor.Green:
                foreach (var winner in winners)
                {
                    await _repository.AddPointsToUserAsync(winner.IdentityUserId, winner.Amount * 14);
                }
                break;
        }
        return result;
    }
    
    public Task<List<RoulettePlayerToView>> GetPlayersToViewAsync()
    {
        var player = Players.Select(p => p.ToView()).ToList();
        return Task.FromResult(player);
    }

    private void ResetGame()
    {
        AllowBetting = true;
        IsSpinning = false;
    }

}