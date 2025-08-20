namespace RichbetsResurrected.Entities.Games.Slots;

public class SlotsSpinRequest
{
    public int UserId { get; set; }
    public string ConnectionId { get; set; } = string.Empty;
    public int Amount { get; set; }
    public float DelayAmountToWithdraw { get; set; }
}
