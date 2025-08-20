namespace RichbetsResurrected.Entities.Games.Slots;

public class SlotsSpinResult
{
    public bool IsSuccess { get; set; }
    public SymbolEnum[] Symbols { get; set; } = new SymbolEnum[3]; // 1-9
    public string? ErrorMessage { get; set; }
}
