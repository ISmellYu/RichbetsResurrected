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
    
    
    public static readonly Dictionary<SymbolEnum, float> SymbolMultipliers = new()
    {
        {SymbolEnum.Seven, 5f},
        {SymbolEnum.Watermelon, 1f},
        {SymbolEnum.Grape, 1f},
        {SymbolEnum.Lemon, 1f},
        {SymbolEnum.Cherry, 1f},
        {SymbolEnum.Plum, 1f},
        {SymbolEnum.Orange, 1f},
        {SymbolEnum.Diamond, 1f},
        {SymbolEnum.Shooter, 3f},
    }; 
}