using RichbetsResurrected.Core.RouletteAggregate.Entities;

namespace RichbetsResurrected.Infrastructure.Games.Roulette;

public class RouletteService
{
    private List<RoulettePlayer> Players { get; set; } = new();
    private bool IsRunning { get; set; } = false;
    

    public RouletteService()
    {
    }
    
}