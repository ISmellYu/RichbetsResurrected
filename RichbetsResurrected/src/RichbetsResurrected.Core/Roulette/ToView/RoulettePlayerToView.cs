using RichbetsResurrected.Core.Roulette.Entities;

namespace RichbetsResurrected.Core.Roulette.ToView;

public class RoulettePlayerToView
{
    public string UserName { get; set; }
    public int Amount { get; set; }
    public RouletteColor Color { get; set; }
}