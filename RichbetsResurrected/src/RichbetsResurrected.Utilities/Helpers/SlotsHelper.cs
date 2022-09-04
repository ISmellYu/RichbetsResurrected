using RichbetsResurrected.Entities.Games.Slots;

namespace RichbetsResurrected.Utilities.Helpers;

public static class SlotsHelper
{
    public static readonly Dictionary<SymbolEnum, Dictionary<int, float>> Multipliers = new Dictionary<SymbolEnum, Dictionary<int, float>>()
    {
        {
            SymbolEnum.Seven, new Dictionary<int, float>(){
                {2, 7.5f},
                {3, 20f},
            }
        },
        {
            SymbolEnum.Watermelon, new Dictionary<int, float>(){
                {2, 1.75f},
                {3, 2.5f},
            }
        },
        {
            SymbolEnum.Grape, new Dictionary<int, float>(){
                {2, 0.5f},
                {3, 1.5f},
            }
        },
        {
            SymbolEnum.Lemon, new Dictionary<int, float>(){
                {2, 0.5f},
                {3, 1.5f},
            }
        },
        {
            SymbolEnum.Cherry, new Dictionary<int, float>(){
                {2, 0.5f},
                {3, 1.5f},
            }
        },
        {
            SymbolEnum.Plum, new Dictionary<int, float>(){
                {2, 0.5f},
                {3, 1.5f},
            }
        },
        {
            SymbolEnum.Orange, new Dictionary<int, float>(){
                {2, 1f},
                {3, 3f},
            }
        },
        {
            SymbolEnum.Diamond, new Dictionary<int, float>(){
                {2, 7.5f},
                {3, 25f},
            }
        },
        {
            SymbolEnum.Shooter, new Dictionary<int, float>(){
                {2, 25f},
                {3, 100f},
            }
        }
    };
}