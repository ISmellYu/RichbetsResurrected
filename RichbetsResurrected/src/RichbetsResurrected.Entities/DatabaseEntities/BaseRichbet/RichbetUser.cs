using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Entities.Identity.Models;

namespace RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

public class RichbetUser
{
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public int Points { get; set; }
    public float Multiplier { get; set; }
    public bool DailyRedeemed { get; set; }
    
    public ICollection<ActiveItem> ActiveItems { get; set; }
    public ICollection<UserItem> UserItems { get; set; }
}