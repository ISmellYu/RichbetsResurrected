namespace RichbetsResurrected.Core.Roulette.Entities;

public class RouletteInfo
{
    public List<RoulettePlayer> Players { get; set; } 
    public List<RouletteResult> Results { get; set; }
    public bool AllowBetting { get; set; }
    public bool IsRolling { get; set; }
}