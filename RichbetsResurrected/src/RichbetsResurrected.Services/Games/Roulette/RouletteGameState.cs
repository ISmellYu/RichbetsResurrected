using System.Collections.Concurrent;
using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.Roulette;
using RichbetsResurrected.Interfaces.Games.Roulette;
using RichbetsResurrected.Utilities.Constants;

namespace RichbetsResurrected.Services.Games.Roulette;

public class RouletteGameState : IRouletteGameState
{
    private BlockingCollection<RoulettePlayer> Players { get; } = new();
    private ConcurrentDictionary<string, ClientInfo> OnlinePlayers { get; } = new();
    private List<RouletteResult> History { get; } = new();
    private bool IsRunning { get; set; }
    private bool AllowBetting { get; set; }
    private bool IsSpinning { get; set; }
    private decimal TimeLeft { get; set; }
    
    public void SetTimeLeft(decimal timeLeft)
    {
        TimeLeft = timeLeft;
    }
    
    public bool CheckIfRunning()
    {
        return IsRunning;
    }
    
    public void TurnOnBetting()
    {
        AllowBetting = true;
    }

    public void TurnOffBetting()
    {
        AllowBetting = false;
    }
    
    public void TurnOnSpinning()
    {
        IsSpinning = true;
    }
    
    public void TurnOffSpinning()
    {
        IsSpinning = false;
    }
    
    public void TurnOnGame()
    {
        IsRunning = true;
    }
    
    public void TurnOffGame()
    {
        IsRunning = false;
    }
    
    public bool CheckIfCanBet()
    {
        return AllowBetting && IsRunning && !IsSpinning;
    }
    
    public RouletteInfo GetRouletteInfoAsync()
    {
        var rouletteInfo = new RouletteInfo
        {
            Players = Players.ToList(), Results = History.TakeLast(10).ToList(), AllowBetting = AllowBetting, IsRolling = IsSpinning,
            TimeLeft = TimeLeft, OnlinePlayers = GetOnlinePlayers()
        };
        return rouletteInfo;
    }
    
    public List<RoulettePlayer> GetPlayers()
    {
        return Players.ToList();
    }
    
    public List<RouletteResult> GetHistory(int amount)
    {
        return History.TakeLast(amount).ToList();
    }
    
    public List<RouletteResult> GetHistory()
    {
        return History.ToList();
    }

    public void AddPlayer(RoulettePlayer player)
    {
        if (IsInGameColor(player))
        {
            foreach (var roulettePlayer in Players)
                if (roulettePlayer.IdentityUserId == player.IdentityUserId && roulettePlayer.Color == player.Color)
                    roulettePlayer.Amount += player.Amount;
        }
        else
        {
            Players.TryAdd(player);
        }
    }
    
    private void ClearPlayers()
    {
        while (Players.TryTake(out _))
        {
        }
    }
    
    public void ResetGame()
    {
        ClearPlayers();
        AllowBetting = true;
        IsSpinning = false;
    }
    
    public void AddToHistory(RouletteResult result)
    {
        History.Add(result);
    }
    
    private bool IsInGameColor(RoulettePlayer player)
    {
        return Players.Any(p => p.IdentityUserId == player.IdentityUserId && p.Color == player.Color);
    }
    
    public void AddOnlinePlayer(string connId, ClientInfo clientInfo)
    {
        OnlinePlayers.TryAdd(connId, clientInfo);
    }
    
    public void RemoveOnlinePlayer(string connId)
    {
        OnlinePlayers.TryRemove(connId, out _);
    }
    
    public List<ClientInfo> GetOnlinePlayers()
    {
        return OnlinePlayers.Values.DistinctBy(p => p.IdentityUserId).ToList();
    }

}