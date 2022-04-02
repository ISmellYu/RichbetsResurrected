namespace RichbetsResurrected.Core.RouletteAggregate.Entities;

public class RouletteResult
{
    public RouletteResult(int number, int segment, RouletteColor rouletteColor, List<RoulettePlayer> winners, List<RoulettePlayer> losers)
    {
        Number = number;
        Segment = segment;
        RouletteColor = rouletteColor;
        Winners = winners;
        Losers = losers;
    }
    public int Number { get; set; }
    public int Segment { get; set; }
    public RouletteColor RouletteColor { get; set; }
    public List<RoulettePlayer> Winners { get; set; }
    public List<RoulettePlayer> Losers { get; set; }

}