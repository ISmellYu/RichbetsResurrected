using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Communication.Crash.Hub;
using RichbetsResurrected.Entities.Crash;
using RichbetsResurrected.Entities.Roulette;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Games;
using RichbetsResurrected.Interfaces.Games.Crash;
using RichbetsResurrected.Utilities.Constants;
using RichbetsResurrected.Utilities.Helpers;

namespace RichbetsResurrected.Services.Games.Crash;

public class CrashService : ICrashService
{
    private readonly ICrashGameState _crashGameState;
    private readonly IRichbetRepository _richbetRepository;
    
    public ICrashGameState GameState => _crashGameState;
    public CrashService(ICrashGameState crashGameState, IRichbetRepository richbetRepository)
    {
        _crashGameState = crashGameState;
        _richbetRepository = richbetRepository;
    }

    public async Task<CrashJoinResult> JoinCrashAsync(CrashPlayer crashPlayer)
    {
        if (_crashGameState.CheckIfCanBet())
        {
            return new CrashJoinResult()
            {
                IsSuccess = false,
                Error = new CrashError()
                {
                    Message = "You cannot bet at this time"
                },
                Player = crashPlayer
            };
        }
        
        if (crashPlayer.Amount <= 0)
        {
            return new CrashJoinResult()
            {
                IsSuccess = false,
                Error = new CrashError()
                {
                    Message = "You cannot bet with a negative amount"
                },
                Player = crashPlayer
            };
        }
        
        var points = await _richbetRepository.GetPointsFromUserAsync(crashPlayer.IdentityUserId);

        if (points - crashPlayer.Amount < 0)
            return new CrashJoinResult()
            {
                IsSuccess = false,
                Error = new CrashError()
                {
                    Message = "You don't have enough points"
                },
                Player = crashPlayer
            };


        await _richbetRepository.RemovePointsFromUserAsync(crashPlayer.IdentityUserId, crashPlayer.Amount);
        
        _crashGameState.AddPlayer(crashPlayer);
        
        return new CrashJoinResult()
        {
            IsSuccess = true,
            Error = null,
            Player = crashPlayer
        };
    }
    
    // TODO: Add cashout method
}