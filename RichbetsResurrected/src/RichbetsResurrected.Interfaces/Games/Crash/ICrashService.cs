using RichbetsResurrected.Entities.Crash;

namespace RichbetsResurrected.Interfaces.Games.Crash;

public interface ICrashService
{
    ICrashGameState GameState { get; }
    Task<CrashJoinResult> JoinCrashAsync(CrashPlayer crashPlayer);
}