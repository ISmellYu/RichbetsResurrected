using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

namespace RichbetsResurrected.Entities.DatabaseEntities.Statistics;

public class Statistic
{
    public int WinAmount { get; set; }
    public int LoseAmount { get; set; }
    
    public int RichetUserId { get; set; }
    public RichbetUser RichetUser { get; set; }
}