using System.Collections.Concurrent;
using RichbetsResurrected.Core.Roulette.Entities;

namespace RichbetsResurrected.Infrastructure.Games.Roulette;

public class RouletteService
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
    
    public async Task AddPlayerAsync(RoulettePlayer player)
    {
        Players.Add(player);
    }
    
    public bool CheckIfCanBet()
    {
        return AllowBetting && IsRunning && !IsSpinning;
    }

}