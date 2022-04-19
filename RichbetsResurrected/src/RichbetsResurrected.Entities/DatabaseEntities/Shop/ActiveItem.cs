using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class ActiveItem
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public int RichetUserId { get; set; }
    public RichbetUser RichetUser { get; set; }
}