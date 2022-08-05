using RichbetsResurrected.Entities.Games.Slots;

namespace RichbetsResurrected.Utilities.Constants;

public static class CrashConfigs
{
    public const int PercentValueToCrashInstantly = 2;
    public const int TimeForUsersToBet = 10;
}

public static class RouletteConfigs
{
    public const int TotalSegments = 37;
    public const int SpinDuration = 3;
    public const int TimeForUsersToBet = 15;
}

public static class SlotsConfig
{
    public const int MinDelay = 1;
    public const int MaxDelay = 5;
    public const int MinBet = 10;
    public const int Columns = 3;
    public const int MinSymbolValue = 1;
    public const int MaxSymbolValue = 9;
    
    
    public static Dictionary<SymbolEnum, float> SymbolMultipliers = new()
    {
        {SymbolEnum.Seven, 0.5f},
        {SymbolEnum.Watermelon, 0.5f},
        {SymbolEnum.Grape, 0.5f},
        {SymbolEnum.Lemon, 0.5f},
        {SymbolEnum.Cherry, 0.5f},
        {SymbolEnum.Plum, 0.5f},
        {SymbolEnum.Orange, 0.5f},
        {SymbolEnum.Diamond, 0.5f},
        {SymbolEnum.Shooter, 0.5f},
    }; 
}