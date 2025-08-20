using RichbetsResurrected.Entities.Client;

namespace RichbetsResurrected.Entities.Games.Crash;

public class CrashInfo
{
    public List<CrashPlayer> Players { get; set; }
    public List<ClientInfo> OnlinePlayers { get; set; }
    public List<CrashResult> Results { get; set; }
    public List<decimal> Multipliers { get; set; }
    public decimal Multiplier { get; set; }
    public decimal TimeLeft { get; set; }

    public bool AllowPlacingBets { get; set; }
    public bool AllowRemovingBets { get; set; }
    public bool Crashed { get; set; }
    public bool GameStarted { get; set; }
    public bool Running { get; set; }
}
