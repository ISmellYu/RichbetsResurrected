using RichbetsResurrected.Core.Roulette.ToView;

namespace RichbetsResurrected.Core.Roulette.Entities;

public class RoulettePlayer
{
    public int IdentityUserId { get; set; }
    public string DiscordUserId { get; set; }
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public RouletteColor Color { get; set; }
    public string ColorName => Color.ToString();
    public int Amount { get; set; }
    
    public RoulettePlayerToView ToView()
    {
        return new RoulettePlayerToView
        {
            UserName = this.UserName,
            Color = this.Color,
            Amount = this.Amount
        };
    }
}