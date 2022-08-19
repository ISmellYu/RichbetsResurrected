using RichbetsResurrected.Entities.Games.Slots;

namespace RichbetsResurrected.Utilities.Helpers;

public static class SlotsHelper
{
    public static readonly Dictionary<SymbolEnum, Dictionary<int, float>> Multipliers = new Dictionary<SymbolEnum, Dictionary<int, float>>()
    {
        {
            SymbolEnum.Seven, new Dictionary<int, float>(){
                {2, 40f},
                {3, 100f},
            }
        },
        {
            SymbolEnum.Watermelon, new Dictionary<int, float>(){
                {2, 4f},
                {3, 10f},
            }
        },
        {
            SymbolEnum.Grape, new Dictionary<int, float>(){
                {2, 3f},
                {3, 6f},
            }
        },
        {
            SymbolEnum.Lemon, new Dictionary<int, float>(){
                {2, 2f},
                {3, 3.5f},
            }
        },
        {
            SymbolEnum.Cherry, new Dictionary<int, float>(){
                {2, 1.5f},
                {3, 3f},
            }
        },
        {
            SymbolEnum.Plum, new Dictionary<int, float>(){
                {2, 2f},
                {3, 3.5f},
            }
        },
        {
            SymbolEnum.Orange, new Dictionary<int, float>(){
                {2, 2.5f},
                {3, 5f},
            }
        },
        {
            SymbolEnum.Diamond, new Dictionary<int, float>(){
                {2, 20f},
                {3, 60f},
            }
        },
        {
            SymbolEnum.Shooter, new Dictionary<int, float>(){
                {2, 80f},
                {3, 350f},
            }
        }
    };
}