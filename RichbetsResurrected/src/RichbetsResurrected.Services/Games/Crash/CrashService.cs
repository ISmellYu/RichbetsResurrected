using RichbetsResurrected.Entities.Crash;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Games.Crash;

namespace RichbetsResurrected.Services.Games.Crash;

public class CrashService : ICrashService
{
    private readonly IRichbetRepository _richbetRepository;
    public CrashService(ICrashGameState crashGameState, IRichbetRepository richbetRepository)
    {
        GameState = crashGameState;
        _richbetRepository = richbetRepository;
    }

    public ICrashGameState GameState { get; }

    public async Task<CrashJoinResult> JoinCrashAsync(CrashPlayer crashPlayer)
    {
        if (GameState.CheckIfCanBet())
            return new CrashJoinResult
            {
                IsSuccess = false,
                Error = new CrashError
                {
                    Message = "You cannot bet at this time"
                },
                Player = crashPlayer
            };

        if (crashPlayer.Amount <= 0)
            return new CrashJoinResult
            {
                IsSuccess = false,
                Error = new CrashError
                {
                    Message = "You cannot bet with a negative amount"
                },
                Player = crashPlayer
            };

        var points = await _richbetRepository.GetPointsFromUserAsync(crashPlayer.IdentityUserId);

        if (points - crashPlayer.Amount < 0)
            return new CrashJoinResult
            {
                IsSuccess = false,
                Error = new CrashError
                {
                    Message = "You don't have enough points"
                },
                Player = crashPlayer
            };


        await _richbetRepository.RemovePointsFromUserAsync(crashPlayer.IdentityUserId, crashPlayer.Amount);

        GameState.AddPlayer(crashPlayer);

        return new CrashJoinResult
        {
            IsSuccess = true, Error = null, Player = crashPlayer
        };
    }

    // TODO: Add cashout method
}