using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Communication.Roulette.Events;
using RichbetsResurrected.Communication.Roulette.Hub;
using RichbetsResurrected.Entities.Roulette;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Games.Roulette;
using RichbetsResurrected.Utilities.Constants;
using RichbetsResurrected.Utilities.Helpers;

namespace RichbetsResurrected.Services.Games.Roulette;

public class RouletteService : IRouletteService
{
    private readonly IMediator _mediator;
    private IRichbetRepository _repository;
    private readonly IRouletteGameState _gameState;
    private readonly ILifetimeScope _hubLifetimeScope;
    
    public IRouletteGameState GameState => _gameState;

    public RouletteService(IRichbetRepository repository, IMediator mediator, IRouletteGameState gameState, ILifetimeScope hubLifetimeScope)
    {
        _repository = repository;
        _mediator = mediator;
        _gameState = gameState;
        _hubLifetimeScope = hubLifetimeScope;
    }
    
    public async Task<RouletteJoinResult> AddPlayerAsync(RoulettePlayer player)
    {
        if (!_gameState.CheckIfCanBet())
        {
            return new RouletteJoinResult()
            {
                IsSuccess = false,
                Error = new RouletteError()
                {
                    Message = "You cannot bet at this time"
                },
                Player = player
            };
        }
        
        if (player.Amount <= 0)
        {
            return new RouletteJoinResult()
            {
                IsSuccess = false,
                Error = new RouletteError()
                {
                    Message = "You cannot bet with a negative amount"
                },
                Player = player
            };
        }
        
        var points = await _repository.GetPointsFromUserAsync(player.IdentityUserId);

        if (points - player.Amount < 0)
            return new RouletteJoinResult
            {
                IsSuccess = false,
                Error = new RouletteError
                {
                    Message = "You don't have enough points"
                },
                Player = player
            };


        await _repository.RemovePointsFromUserAsync(player.IdentityUserId, player.Amount);
        _gameState.AddPlayer(player);
        await SendJoinPlayerToClientsAsync(player);
        return new RouletteJoinResult
        {
            IsSuccess = true, Error = null, Player = player
        };
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
                _gameState.AddToHistory(result);
                await SendEndRouletteToClientsAsync(result);
                await Task.Delay(2000); // Wait for 2 seconds before starting again roulette
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
            _repository = scope.Resolve<IRichbetRepository>();
            switch (winColor)
            {
                case RouletteColor.Black or RouletteColor.Red:
                    foreach (var winner in winners) await _repository.AddPointsToUserAsync(winner.IdentityUserId, winner.Amount * 2);
                    break;
                case RouletteColor.Green:
                    foreach (var winner in winners) await _repository.AddPointsToUserAsync(winner.IdentityUserId, winner.Amount * 14);
                    break;
            }
        }
        
        return result;
    }
}