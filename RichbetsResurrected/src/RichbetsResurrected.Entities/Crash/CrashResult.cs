namespace RichbetsResurrected.Entities.Crash;

public class CrashResult
{
    public List<CrashPlayer> Winners { get; set; }
    public List<CrashPlayer> Losers { get; set; }
    public decimal Multiplier { get; set; }
}