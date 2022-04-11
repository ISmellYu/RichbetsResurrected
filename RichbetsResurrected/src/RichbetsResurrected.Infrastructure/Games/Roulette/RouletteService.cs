﻿using System.Collections.Concurrent;
using MediatR;
using Microsoft.Extensions.Hosting;
using RichbetsResurrected.Core.Interfaces;
using RichbetsResurrected.Core.Interfaces.Games;
using RichbetsResurrected.Core.Interfaces.Games.Roulette;
using RichbetsResurrected.Core.Roulette.Entities;
using RichbetsResurrected.Core.Roulette.Events;
using RichbetsResurrected.Core.Roulette.ToView;
using RichbetsResurrected.Infrastructure.BaseRichbet;

namespace RichbetsResurrected.Infrastructure.Games.Roulette;

public class RouletteService : IRouletteService
{
    private BlockingCollection<RoulettePlayer> Players { get; set; } = new();
    private List<RouletteResult> History { get; set; } = new();
    private bool IsRunning { get; set; } = false;
    private bool AllowBetting { get; set; } = false;
    private bool IsSpinning { get; set; } = false;
    
    
    private readonly IRichbetRepository _repository;
    private readonly IMediator _mediator;

    public RouletteService(IRichbetRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }
    
    public void TurnOnBetting()
    {
        AllowBetting = true;
    }
    
    public void TurnOffBetting()
    {
        AllowBetting = false;
    }
    
    public async Task<RouletteJoinResult> AddPlayerAsync(RoulettePlayer player)
    {
        var points = await _repository.GetPointsFromUserAsync(player.IdentityUserId);
        
        if (points - player.Amount < 0) 
            return new RouletteJoinResult()
            {
                IsSuccess = false,
                Error = new RouletteError(){Message = "You don't have enough points"},
                Player = player
            };
        
        
        await _repository.RemovePointsFromUserAsync(player.IdentityUserId, player.Amount);
        AddToPlayerList(player);
        // await SendJoinConfirmationToPlayerAsync(player);
        await SendJoinPlayerToClientsAsync(player);
        return new RouletteJoinResult()
        {
            IsSuccess = true,
            Error = null,
            Player = player
        };
    }
    
    private bool IsInGameColor(RoulettePlayer player)
    {
        return Players.Any(p => p.IdentityUserId == player.IdentityUserId && p.Color == player.Color);
    } 
    
    private void AddToPlayerList(RoulettePlayer player)
    {
        if (IsInGameColor(player))
        {
            foreach (var roulettePlayer in Players)
            {
                if (roulettePlayer.IdentityUserId == player.IdentityUserId && roulettePlayer.Color == player.Color)
                {
                    roulettePlayer.Amount += player.Amount;
                }
            }
        }
        else
        {
            Players.TryAdd(player);
        }
    }
    
    public bool CheckIfCanBet()
    {
        return AllowBetting && IsRunning && !IsSpinning;
    }
    public async Task StartAsync()
    {
        try
        {
            IsRunning = true;
            while (true)
            {
                ResetGame();
                await WaitForPlayersAsync();
                var winNumber = GetRandomWinNumber();
                await SpinAsync(winNumber);
                var winColor = RouletteConstants.GetRouletteColorForNumber(winNumber);
                var result = await AwardWinnersAsync(winNumber, winColor);
                AddToHistory(result);
                await SendEndRouletteToClientsAsync(result);
            }
        }
        catch (Exception e)
        {
            IsRunning = false;
        }
    }

    private async Task WaitForPlayersAsync()
    {
        TurnOnBetting();
        for (int i = 0; i <= 15; i++)
        {
            await SendUpdateTimerToClientsAsync(15000 - 1000 * i);
            await Task.Delay(1000);
        }
        TurnOffBetting();
        await Task.Delay(1000); // Just to make sure every bet is done adding
    }

    private async Task SpinAsync(int winNumber)
    {
        IsSpinning = true;
        var segment = RouletteConstants.GetSegmentForNumber(winNumber);
        var stopAt = RouletteConstants.GetRandomAngleForSegment(segment, RouletteConstants.TotalSegments);
        await StartAnimationForClientsAsync(stopAt);
        await Task.Delay(RouletteConstants.SpinDuration);
        IsSpinning = false;
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
        var history = History.TakeLast(10).ToList();
        return _mediator.Publish(new EndRouletteNotification(history, result));
    }

    public async Task<RouletteInfo> GetRouletteInfoAsync()
    {
        var rouletteInfo = new RouletteInfo
        {
            Players = Players.ToList(),
            Results = History.TakeLast(10).ToList(),
            AllowBetting = AllowBetting,
            IsRolling = IsSpinning
        };
        return rouletteInfo;
    }

    private int GetRandomWinNumber()
    {
        return Random.Shared.Next(0, RouletteConstants.TotalSegments - 1);
    }
    
    private void AddToHistory(RouletteResult result)
    {
        History.Add(result);
    }

    private async Task<RouletteResult> AwardWinnersAsync(int number, RouletteColor winColor)
    {
        var winners = Players.Where(p => p.Color == winColor).ToList();
        var losers = Players.Where(p => p.Color != winColor).ToList();
        
        var result = new RouletteResult(number, winColor, winners.ToList(), losers.ToList());
        switch (winColor)
        {
            case RouletteColor.Black or RouletteColor.Red:
                foreach (var winner in winners)
                {
                    await _repository.AddPointsToUserAsync(winner.IdentityUserId, winner.Amount * 2);
                }
                break;
            case RouletteColor.Green:
                foreach (var winner in winners)
                {
                    await _repository.AddPointsToUserAsync(winner.IdentityUserId, winner.Amount * 14);
                }
                break;
        }
        return result;
    }
    
    public List<RoulettePlayerToView> GetPlayersToViewAsync()
    {
        var players = Players.Select(p => p.ToView()).ToList();
        return players;
    }

    private void ClearPlayers()
    {
        while (Players.TryTake(out _)){}
    }
    private void ResetGame()
    {
        ClearPlayers();
        AllowBetting = true;
        IsSpinning = false;
    }

}