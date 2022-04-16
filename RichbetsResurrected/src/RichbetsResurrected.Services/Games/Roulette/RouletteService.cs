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
    private readonly IRichbetRepository _repository;
    private readonly IRouletteGameState _gameState;

    public IRouletteGameState GameState => _gameState;

    public RouletteService(IRichbetRepository repository, IRouletteGameState gameState)
    {
        _repository = repository;
        _gameState = gameState;
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
        // await SendJoinPlayerToClientsAsync(player);
        return new RouletteJoinResult
        {
            IsSuccess = true, Error = null, Player = player
        };
    }
}