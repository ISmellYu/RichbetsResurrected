namespace RichbetsResurrected.Entities.Roulette;

public class RouletteResult
{
    public RouletteResult(int number, RouletteColor rouletteColor, List<RoulettePlayer> winners, List<RoulettePlayer> losers)
    {
        Number = number;
        RouletteColor = rouletteColor;
        Winners = winners;
        Losers = losers;
    }
    public int Number { get; set; }
    public RouletteColor RouletteColor { get; set; }
    public string Color => RouletteColor.ToString();
    public List<RoulettePlayer> Winners { get; set; }
    public List<RoulettePlayer> Losers { get; set; }
}