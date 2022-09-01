using Autofac;
using MediatR;
using RichbetsResurrected.Communication.Slots.Events;
using RichbetsResurrected.Entities.Games.Slots;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Games.Slots;
using RichbetsResurrected.Interfaces.Utils;
using RichbetsResurrected.Services.Utils;
using RichbetsResurrected.Utilities.Constants;
using RichbetsResurrected.Utilities.Helpers;

namespace RichbetsResurrected.Services.Games.Slots;

public class SlotsService : ISlotsService
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IRichbetRepository _richbetRepository;
    private readonly IBackgroundTasks _backgroundTasks;
    
    public SlotsService(ILifetimeScope lifetimeScope, IRichbetRepository richbetRepository, IBackgroundTasks backgroundTasks)
    {
        _lifetimeScope = lifetimeScope;
        _richbetRepository = richbetRepository;
        _backgroundTasks = backgroundTasks;
    }
    
    public async Task<SlotsSpinResult> SpinAsync(SlotsSpinRequest request, string connectionId)
    {
        var richbetUser = await _richbetRepository.GetRichbetUserAsync(request.UserId);

        if (request.Amount <= 0)
        {
            return new SlotsSpinResult
            {
                IsSuccess = false, ErrorMessage = "Bet amount must be greater than 0"
            };
        }
        
        if (richbetUser.Points < request.Amount)
        {
            return new SlotsSpinResult
            {
                IsSuccess = false, ErrorMessage = "You don't have enough points to bet this amount"
            };
        }
        
        if (request.DelayAmountToWithdraw is > 5 or < 1)
        {
            return new SlotsSpinResult
            {
                IsSuccess = false, ErrorMessage = "Delay amount to withdraw must be between 1 and 5"
            };
        }

        if (request.Amount < SlotsConfig.MinBet)
        {
            return new SlotsSpinResult
            {
                IsSuccess = false, ErrorMessage = "You must bet at least " + SlotsConfig.MinBet + " points"
            };
        }
        
        await _richbetRepository.RemovePointsFromUserAsync(request.UserId, request.Amount);
        
        var generatedSymbols = GetRandomRoll(SlotsConfig.Columns);
        
        var slotSpinResult = new SlotsSpinResult
        {
            IsSuccess = true,
            Symbols = generatedSymbols
        };

        var withdrawResult = WinHandler.Handle(generatedSymbols, request.Amount);

        _backgroundTasks.DelaySlotsWithdrawalAsync(withdrawResult, Convert.ToInt32(1000 * request.DelayAmountToWithdraw), request.UserId,
            connectionId);
        
        return slotSpinResult;
    }
    
    private SymbolEnum[] GetRandomRoll(int amount)
    {
        var numbers = new SymbolEnum[amount];
        var enumValues = Enum.GetValues(typeof(SymbolEnum));
        for (var i = 0; i < amount; i++)
        {
            // get random number from enum
            var randomNumber = (SymbolEnum)enumValues.GetValue(new Random().Next(0, enumValues.Length));
            numbers[i] = randomNumber;
        }
        return numbers;
    }

    private SlotsWithdrawResult GetWithdrawResult(IReadOnlyList<SymbolEnum> symbolArray, int amount)
    {
        // check if all symbols are the same
        if (symbolArray.All(p => p == symbolArray[0]))
        {
            var multiplier = GetMultiplierFromSymbol(symbolArray[0]);
            multiplier *= 3;    // 3x multiplier for 3 symbols in a row
            return new SlotsWithdrawResult
            {
                IsWin = true, Multiplier = multiplier, WinAmount = Convert.ToInt32(multiplier * amount) 
            };
        }
        
        // check if 2 symbols are the same
        SymbolEnum? symbolPicked = null;
        var isTwoSame = false;
        for (var i = 0; i < symbolArray.Count - 1; i++)
        {
            if (symbolArray.Count(p => p == symbolArray[i]) != 2) continue;
            symbolPicked = symbolArray[i];
            isTwoSame = true;
            break;
        }

        if (isTwoSame && symbolPicked != null)
        {
            var multiplier = GetMultiplierFromSymbol(symbolPicked.Value);
            multiplier *= 2;    // 2x multiplier for 2 symbols in a row
            return new SlotsWithdrawResult
            {
                IsWin = true, Multiplier = multiplier, WinAmount = Convert.ToInt32(multiplier * amount) 
            };
        }
        
        return new SlotsWithdrawResult
        {
            IsWin = false, Multiplier = null, WinAmount = null
        };
    }
    
    private float GetMultiplierFromSymbol(SymbolEnum symbol)
    {
        return SlotsConfig.SymbolMultipliers[symbol];
    }

}

public static class WinHandler
{
    public static SlotsWithdrawResult Handle(IReadOnlyList<SymbolEnum> symbols, int amount)
    {
        // Check if all symbols are different
        if (symbols.Count(p => p == symbols[0]) == 1 && symbols.Count(p => p == symbols[1]) == 1)
        {
            return new SlotsWithdrawResult
            {
                IsWin = false, Multiplier = null, WinAmount = null
            };
        }
        
        var mostCommonSymbol = symbols.GroupBy(p => p).OrderByDescending(p => p.Count()).First().Key;
        var iOccurences = symbols.Count(p => p == mostCommonSymbol);
        var multiplier = SlotsHelper.Multipliers[mostCommonSymbol][iOccurences];
        
        return new SlotsWithdrawResult
        {
            IsWin = true, Multiplier = multiplier, WinAmount = Convert.ToInt32(multiplier * amount)
        };
    }
}