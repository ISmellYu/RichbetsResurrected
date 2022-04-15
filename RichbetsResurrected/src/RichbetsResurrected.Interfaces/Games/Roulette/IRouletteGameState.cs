using RichbetsResurrected.Entities.Roulette;

namespace RichbetsResurrected.Interfaces.Games.Roulette;

public interface IRouletteGameState
{
    public void SetTimeLeft(decimal timeLeft);
    public bool CheckIfRunning();
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
}