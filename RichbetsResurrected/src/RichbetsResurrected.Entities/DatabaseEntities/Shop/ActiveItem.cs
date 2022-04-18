namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class ActiveItem
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ItemId { get; set; }
    public int RichbetUserId { get; set; }
}