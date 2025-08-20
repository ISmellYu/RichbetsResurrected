namespace RichbetsResurrected.Entities.Games.Roulette;

public class RouletteResult
{
    public RouletteResult(int number, RouletteColor color, List<RoulettePlayer> winners, List<RoulettePlayer> losers)
    {
        Number = number;
        Color = color;
        Winners = winners;
        Losers = losers;
    }

    public int Number { get; set; }
    public RouletteColor Color { get; set; }
    public string ColorName => Color.ToString();
    public List<RoulettePlayer> Winners { get; set; }
    public List<RoulettePlayer> Losers { get; set; }
}
