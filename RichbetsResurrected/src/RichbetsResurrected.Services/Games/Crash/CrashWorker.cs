using Autofac;
using RichbetsResurrected.Entities.Crash;
using RichbetsResurrected.Interfaces.Games.Crash;
using RichbetsResurrected.Utilities.Constants;
using RichbetsResurrected.Utilities.Helpers;

namespace RichbetsResurrected.Services.Games.Crash;

public class CrashWorker : ICrashWorker
{
    private readonly ICrashGameState _gameState;
    private readonly ILifetimeScope _hubLifetimeScope;

    public CrashWorker(ICrashGameState gameState, ILifetimeScope hubLifetimeScope)
    {
        _gameState = gameState;
        _hubLifetimeScope = hubLifetimeScope;
    }

    public async Task StartAsync()
    {
        if (_gameState.IsRunning())
            return;

        try
        {
            _gameState.TurnOnRunning();
            while (true)
            {
                _gameState.ResetGame();
                await WaitForPlayersAsync();
                var maxMultiplier = CrashHelper.RandomMultiplier();
                maxMultiplier = 1.01m;
                _gameState.TurnOnGameStarted();
                await StartCountingAsync(maxMultiplier);
                // TODO: Change it later to normal value(wait after bets to show losers etc)
                await Task.Delay(1000);
                var result = GetResult();
                _gameState.AddToHistory(result);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        _gameState.TurnOffRunning();
    }

    private async Task WaitForPlayersAsync()
    {
        _gameState.TurnOnPlacingBets();
        // TODO: Change it later to normal value
        for (decimal i = 1; i >= 0; i -= 0.01m)
        {
            _gameState.SetTimeLeft(i);
            // await SendUpdateTimerToClientsAsync(i);
            await Task.Delay(10);
        }
        _gameState.TurnOffPlacingBets();
        await Task.Delay(100); // Just to make sure every bet is done adding
    }

    private async Task StartCountingAsync(decimal maxMultiplier)
    {
        var multiplier = 1m;
        const decimal step = 0.01m;

        if (maxMultiplier != 1.0m)
        {
            _gameState.TurnOnRemovingBets();
            while (multiplier <= maxMultiplier)
            {
                if (multiplier == maxMultiplier)
                    break;
                
                _gameState.SetMultiplier(multiplier);
                _gameState.AddToMultipliers(multiplier);

                multiplier += (int) multiplier * step;
                
                await Task.Delay(100);
            }
        }

        

        _gameState.TurnOffRemovingBets();
        _gameState.TurnOnCrashed();
        _gameState.SetMultiplier(multiplier);
        _gameState.AddToMultipliers(multiplier);

    }

    private CrashResult GetResult()
    {
        var result = new CrashResult
        {
            Losers = _gameState.GetPlayers().Where(p => p.Cashouted == false).ToList(),
            Winners = _gameState.GetPlayers().Where(p => p.Cashouted).ToList(),
            Multiplier = _gameState.GetMultiplier()
        };
        return result;
    }

    // TODO: Add autocashout method
}