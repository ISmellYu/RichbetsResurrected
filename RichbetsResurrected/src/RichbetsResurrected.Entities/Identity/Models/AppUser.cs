using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

namespace RichbetsResurrected.Entities.Identity.Models;

public class AppUser : IdentityUser<int>
{
    public RichbetUser RichbetUser { get; set; }
}