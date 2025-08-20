namespace RichbetsResurrected.Entities.Games.Crash;

public class CrashPlayer
{
    public int IdentityUserId { get; set; }
    public string DiscordUserId { get; set; }
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public int Amount { get; set; }
    public bool Cashouted { get; set; }
    public decimal WhenCashouted { get; set; }
}
