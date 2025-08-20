namespace RichbetsResurrected.Entities.Games.Roulette;

public class RouletteJoinResult
{
    public bool IsSuccess { get; set; }
    public RouletteError? Error { get; set; }
    public RoulettePlayer? Player { get; set; }
}
