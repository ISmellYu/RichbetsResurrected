using RichbetsResurrected.Entities.Games.Slots;

namespace RichbetsResurrected.Utilities.Helpers;

public static class SlotsHelper
{
    public static readonly Dictionary<SymbolEnum, Dictionary<int, float>> Multipliers = new()
    {
        {SymbolEnum.Seven, new Dictionary<int, float> {{2, 2.5f}, {3, 5f}}},
        {SymbolEnum.Watermelon, new Dictionary<int, float> {{2, 1f}, {3, 1.75f}}},
        {SymbolEnum.Grape, new Dictionary<int, float> {{2, 0.25f}, {3, 1.25f}}},
        {SymbolEnum.Lemon, new Dictionary<int, float> {{2, 0.25f}, {3, 1.25f}}},
        {SymbolEnum.Cherry, new Dictionary<int, float> {{2, 0.25f}, {3, 1.25f}}},
        {SymbolEnum.Plum, new Dictionary<int, float> {{2, 0.25f}, {3, 1.25f}}},
        {SymbolEnum.Orange, new Dictionary<int, float> {{2, 0.5f}, {3, 1.5f}}},
        {SymbolEnum.Diamond, new Dictionary<int, float> {{2, 2f}, {3, 4f}}},
        {SymbolEnum.Shooter, new Dictionary<int, float> {{2, 5f}, {3, 25f}}}
    };
}
