using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.Crash;

namespace RichbetsResurrected.Interfaces.Games.Crash;

public interface ICrashGameState
{
    void TurnOnPlacingBets();
    void TurnOffPlacingBets();
    void TurnOnRemovingBets();
    void TurnOffRemovingBets();
    void TurnOnCrashed();
    void TurnOffCrashed();
    void TurnOnGameStarted();
    void TurnOffGameStarted();
    void TurnOnRunning();
    void TurnOffRunning();
    bool IsRunning();
    bool IsCrashed();
    bool IsGameStarted();
    bool IsRemovingBetsAllowed();
    bool IsPlacingBetsAllowed();
    void SetMultiplier(decimal multiplier);
    void SetTimeLeft(decimal timeLeft);
    decimal GetTimeLeft();
    decimal GetMultiplier();
    List<CrashPlayer> GetPlayers();
    List<ClientInfo> GetOnlinePlayers();
    List<CrashResult> GetResults(int amount);
    void AddToHistory(CrashResult result);
    void AddPlayer(CrashPlayer player);
    CrashCashoutResult Cashout(int identityUserId, decimal? desiredMultiplier = null);
    void AddOnlinePlayer(string connId, ClientInfo clientInfo);
    void RemoveOnlinePlayer(string connId);
    bool IsInGame(CrashPlayer crashPlayer);
    bool IsInGame(int identityUserId);
    bool AlreadyCashouted(int identityUserId);
    void ClearPlayers();
    void ResetGame();
    bool CheckIfCanBet();
    void AddToMultipliers(decimal multiplier);
    void ClearMultipliers();
    CrashInfo GetCrashInfo();
}