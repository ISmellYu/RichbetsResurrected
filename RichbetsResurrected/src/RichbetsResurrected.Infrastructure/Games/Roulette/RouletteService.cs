using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using RichbetsResurrected.Core.Interfaces.Games;
using RichbetsResurrected.Core.Roulette.Entities;

namespace RichbetsResurrected.Infrastructure.Games.Roulette;

public class RouletteService : IStartableGame
{
    private BlockingCollection<RoulettePlayer> Players { get; set; } = new();
    private List<RouletteResult> History { get; set; } = new();
    private bool IsRunning { get; set; } = false;
    private bool AllowBetting { get; set; } = false;
    private bool IsSpinning { get; set; } = false;

    public RouletteService()
    {
        IsRunning = true;
        AllowBetting = true;
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
                // TODO: Add result to history and send updates to players
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
        // Implement animation
    }
    
    private int GetRandomWinNumber()
    {
        return Random.Shared.Next(0, RouletteConstants.TotalSegments - 1);
    }

    private async Task<RouletteResult> AwardWinnersAsync(int number, RouletteColor winColor)
    {
        var winners = Players.Where(p => p.Color == winColor);
        var losers = Players.Where(p => p.Color != winColor);
        
        var result = new RouletteResult(number, winColor, winners.ToList(), losers.ToList());
        // TODO: Send result to clients
        // TODO: Award winners
        return result;
    }

    private void ResetGame()
    {
        AllowBetting = true;
        IsSpinning = false;
    }

}