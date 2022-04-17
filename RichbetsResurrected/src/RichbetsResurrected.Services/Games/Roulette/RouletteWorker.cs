using Autofac;
using MediatR;
using RichbetsResurrected.Communication.Roulette.Events;
using RichbetsResurrected.Entities.Roulette;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Games.Roulette;
using RichbetsResurrected.Utilities.Constants;
using RichbetsResurrected.Utilities.Helpers;

namespace RichbetsResurrected.Services.Games.Roulette;

public class RouletteWorker : IRouletteWorker
{
    private readonly IMediator _mediator;
    private readonly IRouletteGameState _gameState;
    private readonly ILifetimeScope _hubLifetimeScope;
    
    public RouletteWorker(IRouletteGameState gameState, ILifetimeScope hubLifetimeScope, IMediator mediator)
    {
        _gameState = gameState;
        _hubLifetimeScope = hubLifetimeScope;
        _mediator = mediator;
    }
    
    public async Task StartAsync()
    {
        if (_gameState.CheckIfRunning())
            return;
        
        try
        {
            _gameState.TurnOnGame();
            while (true)
            {
                _gameState.ResetGame();
                await WaitForPlayersAsync();
                var winNumber = GetRandomWinNumber();
                await SpinAsync(winNumber);
                await WaitForAnimationEndAsync(RouletteConfigs.SpinDuration * 1000);
                var winColor = RouletteHelper.GetRouletteColorForNumber(winNumber);
                var result = await AwardWinnersAsync(winNumber, winColor);
                await SendEndRouletteToClientsAsync(result);
                await Task.Delay(2000); // Wait for 2 seconds before starting again roulette
                _gameState.AddToHistory(result);
            }
        }
        catch (Exception e)
        {
            _gameState.TurnOffGame();
        }
    }

    private async Task WaitForPlayersAsync()
    {
        _gameState.TurnOnBetting();
        for (decimal i = RouletteConfigs.TimeForUsersToBet; i >= 0; i -= 0.01m)
        {
            _gameState.SetTimeLeft(i);
            // await SendUpdateTimerToClientsAsync(i);
            await Task.Delay(10);
        }
        _gameState.TurnOffBetting();
        await Task.Delay(100); // Just to make sure every bet is done adding
    }

    private Task SpinAsync(int winNumber)
    {
        _gameState.TurnOnSpinning();
        var segment = RouletteHelper.GetSegmentForNumber(winNumber);
        var stopAt = RouletteHelper.GetRandomAngleForSegment(segment, RouletteConfigs.TotalSegments);
        return StartAnimationForClientsAsync(stopAt);
    }

    private async Task WaitForAnimationEndAsync(int timeInMilliseconds)
    {
        await Task.Delay(timeInMilliseconds);
        _gameState.TurnOffSpinning();
    }

    private Task StartAnimationForClientsAsync(double stopAt)
    {
        return _mediator.Publish(new StartAnimationNotification(stopAt));
    }

    private Task SendJoinPlayerToClientsAsync(RoulettePlayer player)
    {
        return _mediator.Publish(new NewPlayerJoinedNotification(player));
    }

    private Task SendUpdateTimerToClientsAsync(int timeLeft)
    {
        return _mediator.Publish(new UpdateTimerNotification(timeLeft));
    }

    private Task SendEndRouletteToClientsAsync(RouletteResult result)
    {
        var history = _gameState.GetHistory(10);
        return _mediator.Publish(new EndRouletteNotification(history, result));
    }

    private int GetRandomWinNumber()
    {
        return Random.Shared.Next(0, RouletteConfigs.TotalSegments - 1);
    }

    private async Task<RouletteResult> AwardWinnersAsync(int number, RouletteColor winColor)
    {
        var players = _gameState.GetPlayers();
        var winners = players.Where(p => p.Color == winColor).ToList();
        var losers = players.Where(p => p.Color != winColor).ToList();
        
        var result = new RouletteResult(number, winColor, winners.ToList(), losers.ToList());
        await using (var scope = _hubLifetimeScope.BeginLifetimeScope())
        {
            var repository = scope.Resolve<IRichbetRepository>();
            switch (winColor)
            {
                case RouletteColor.Black or RouletteColor.Red:
                    foreach (var winner in winners) await repository.AddPointsToUserAsync(winner.IdentityUserId, winner.Amount * 2);
                    break;
                case RouletteColor.Green:
                    foreach (var winner in winners) await repository.AddPointsToUserAsync(winner.IdentityUserId, winner.Amount * 14);
                    break;
            }
        }
        
        return result;
    }
}