using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.Games.Roulette;

namespace RichbetsResurrected.Interfaces.Games.Roulette;

public interface IRouletteGameState
{
    void SetTimeLeft(decimal timeLeft);
    bool CheckIfRunning();
    void TurnOnBetting();
    void TurnOffBetting();
    void TurnOnSpinning();
    void TurnOffSpinning();
    void TurnOnGame();
    void TurnOffGame();
    bool CheckIfCanBet();
    RouletteInfo GetRouletteInfoAsync();
    List<RoulettePlayer> GetPlayers();
    List<RouletteResult> GetHistory(int amount);
    List<RouletteResult> GetHistory();
    void AddPlayer(RoulettePlayer player);
    void ResetGame();
    void AddToHistory(RouletteResult result);
    void AddOnlinePlayer(string connId, ClientInfo clientInfo);
    void RemoveOnlinePlayer(string connId);
    List<ClientInfo> GetOnlinePlayers();
}
