namespace RichbetsResurrected.Entities.Crash;

public class CrashInfo
{
    public List<CrashResult> Results { get; set; }
    public decimal Multiplier { get; set; }
    public bool AllowPlacingBets { get; set; }
    public bool AllowRemovingBets { get; set; }
    public bool Crashed { get; set; }
    public decimal TimeLeft { get; set; }
}