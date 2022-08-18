using RichbetsResurrected.Entities.DatabaseEntities;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Entities.DatabaseEntities.Statistics;

namespace RichbetsResurrected.Entities.Client;

public class ClientInfo
{
    public int IdentityUserId { get; set; }
    public string DiscordUserId { get; set; }
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public RichbetUser RichbetUser { get; set; }
    public List<Item> EquippedItems { get; set; }
    public int GlobalWin { get; set; }
    public int GlobalLoss { get; set; }
}