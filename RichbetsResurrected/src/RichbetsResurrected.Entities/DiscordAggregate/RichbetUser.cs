namespace RichbetsResurrected.Entities.DiscordAggregate;

public class RichbetUser
{
    public int Id { get; set; }
    public int Points { get; set; }
    public float Multiplier { get; set; }
    public bool DailyRedeemed { get; set; }
}