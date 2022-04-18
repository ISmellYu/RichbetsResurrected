using RichbetsResurrected.Entities.DatabaseEntities;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

namespace RichbetsResurrected.Entities.Client;

public class ClientInfo
{
    public int IdentityUserId { get; set; }
    public string DiscordUserId { get; set; }
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public RichbetUser RichbetUser { get; set; }
}