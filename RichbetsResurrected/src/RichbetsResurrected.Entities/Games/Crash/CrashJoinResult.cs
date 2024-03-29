namespace RichbetsResurrected.Entities.Games.Crash;

public class CrashJoinResult
{
    public bool IsSuccess { get; set; }
    public CrashError? Error { get; set; }
    public CrashPlayer? Player { get; set; }
}