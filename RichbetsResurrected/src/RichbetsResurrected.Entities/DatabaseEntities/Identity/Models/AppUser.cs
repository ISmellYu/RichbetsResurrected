using Microsoft.AspNetCore.Identity;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

namespace RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;

public class AppUser : IdentityUser<int>
{
    public RichbetUser RichbetUser { get; set; }
}
