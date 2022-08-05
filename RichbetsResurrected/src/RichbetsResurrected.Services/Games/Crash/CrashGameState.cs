using System.Collections.Concurrent;
using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.Games.Crash;
using RichbetsResurrected.Interfaces.Games.Crash;

namespace RichbetsResurrected.Services.Games.Crash;

public class CrashGameState : ICrashGameState
{
    private readonly ConcurrentDictionary<string, ClientInfo> _onlinePlayers = new();
    private BlockingCollection<CrashPlayer> Players { get; } = new();
    private List<CrashResult> History { get; } = new();
    private BlockingCollection<decimal> Multipliers { get; } = new();
    
    private decimal Multiplier { get; set; }
    private decimal MaxMultiplier { get; set; }
    private decimal TimeLeft { get; set; }

    private bool AllowPlacingBets { get; set; }
    private bool AllowRemovingBets { get; set; }
    private bool Crashed { get; set; }
    private bool GameStarted { get; set; }
    private bool Running { get; set; }

    public void TurnOnPlacingBets()
    {
        AllowPlacingBets = true;
    }

    public void TurnOffPlacingBets()
    {
        AllowPlacingBets = false;
    }

    public void TurnOnRemovingBets()
    {
        AllowRemovingBets = true;
    }

    public void TurnOffRemovingBets()
    {
        AllowRemovingBets = false;
    }

    public void TurnOnCrashed()
    {
        Crashed = true;
    }

    public void TurnOffCrashed()
    {
        Crashed = false;
    }

    public void TurnOnGameStarted()
    {
        GameStarted = true;
    }

    public void TurnOffGameStarted()
    {
        GameStarted = false;
    }

    public void TurnOnRunning()
    {
        Running = true;
    }

    public void TurnOffRunning()
    {
        Running = false;
    }

    public bool IsRunning()
    {
        return Running;
    }

    public bool IsCrashed()
    {
        return Crashed;
    }

    public bool IsGameStarted()
    {
        return GameStarted;
    }

    public bool IsRemovingBetsAllowed()
    {
        return AllowRemovingBets;
    }

    public bool IsPlacingBetsAllowed()
    {
        return AllowPlacingBets;
    }

    public void SetMultiplier(decimal multiplier)
    {
        Multiplier = multiplier;
    }
    public void SetMaxMultiplier(decimal maxMultiplier)
    {
        MaxMultiplier = maxMultiplier;
    }
    
    public void SetTimeLeft(decimal timeLeft)
    {
        TimeLeft = timeLeft;
    }

    public decimal GetTimeLeft()
    {
        return TimeLeft;
    }

    public decimal GetMultiplier()
    {
        return Multiplier;
    }
    
    public decimal GetMaxMultiplier()
    {
        return MaxMultiplier;
    }

    public List<CrashPlayer> GetPlayers()
    {
        return Players.ToList();
    }

    public List<ClientInfo> GetOnlinePlayers()
    {
        return _onlinePlayers.Values.DistinctBy(p => p.IdentityUserId).ToList();
    }

    public List<CrashResult> GetResults(int amount)
    {
        return History.TakeLast(amount).ToList();
    }

    public void AddToHistory(CrashResult result)
    {
        History.Add(result);
    }

    public void AddPlayer(CrashPlayer player)
    {
        if (IsInGame(player))
            foreach (var crashPlayer in Players)
            {
                if (crashPlayer.IdentityUserId != player.IdentityUserId) continue;
                crashPlayer.Amount += player.Amount;
                return;
            }

        Players.TryAdd(player);
    }
    
    public CrashCashoutResult Cashout(int identityUserId, decimal? desiredMultiplier = null)
    {
        if (!IsInGame(identityUserId))
            return new CrashCashoutResult(){IsSuccess = false, Error = new CrashError(){Message = "You are not in game"}};

        if (AlreadyCashouted(identityUserId))
            return new CrashCashoutResult(){IsSuccess = false, Error = new CrashError(){Message = "You already cashouted"}};

        CrashPlayer player = null;
        foreach (var crashPlayer in Players)
        {
            if (crashPlayer.IdentityUserId != identityUserId) 
                continue;
            
            crashPlayer.Cashouted = true;
            crashPlayer.WhenCashouted = Multiplier;
            if (desiredMultiplier != null && CheckMultiplierCorrectness(desiredMultiplier.Value))
            {
                crashPlayer.WhenCashouted = desiredMultiplier.Value;
                Console.WriteLine($"Desired multiplier is correct {desiredMultiplier.Value}");
            }
            
            player = crashPlayer;
            break;
        }
        
        return new CrashCashoutResult()
        {
            IsSuccess = true,
            Error = null,
            Player = player
        };;
    }
    
    private bool CheckMultiplierCorrectness(decimal multiplier)
    {
        var lastMultiplier = Multipliers.LastOrDefault();
        var secondLastMultiplier = Multipliers.ElementAtOrDefault(Multipliers.Count - 2);
        if (secondLastMultiplier == 0)
            secondLastMultiplier = lastMultiplier;
        
        return secondLastMultiplier <= multiplier && lastMultiplier >= multiplier;
    }

    public void AddOnlinePlayer(string connId, ClientInfo clientInfo)
    {
        _onlinePlayers.TryAdd(connId, clientInfo);
    }

    public void RemoveOnlinePlayer(string connId)
    {
        _onlinePlayers.TryRemove(connId, out _);
    }

    public bool IsInGame(CrashPlayer crashPlayer)
    {
        return Players.Any(p => p.IdentityUserId == crashPlayer.IdentityUserId);
    }
    public bool IsInGame(int identityUserId)
    {
        return Players.Any(p => p.IdentityUserId == identityUserId);
    }
    public bool AlreadyCashouted(int identityUserId)
    {
        return Players.Any(p => p.IdentityUserId == identityUserId && p.Cashouted);
    }

    public void ClearPlayers()
    {
        while (Players.TryTake(out _))
        {
        }
    }

    public void ResetGame()
    {
        ClearMultipliers();
        ClearPlayers();
        TurnOffCrashed();
        SetMultiplier(1);
        TurnOffGameStarted();
        SetMaxMultiplier(1);
    }

    public bool CheckIfCanBet()
    {
        if (!IsRunning())
            return false;
        return IsPlacingBetsAllowed() && !IsRemovingBetsAllowed();
    }

    public void AddToMultipliers(decimal multiplier)
    {
        Multipliers.Add(multiplier);
    }

    public void ClearMultipliers()
    {
        while (Multipliers.TryTake(out _))
        {
        }
    }
    public CrashInfo GetCrashInfo()
    {
        var crashInfo = new CrashInfo
        {
            Players = Players.ToList(),
            Results = History.TakeLast(10).ToList(),
            Multipliers = Multipliers.ToList(),
            Multiplier = Multiplier,
            OnlinePlayers = GetOnlinePlayers(),
            TimeLeft = TimeLeft,
            AllowPlacingBets = AllowPlacingBets,
            AllowRemovingBets = AllowRemovingBets,
            Running = Running,
            Crashed = Crashed,
            GameStarted = GameStarted
        };
        return crashInfo;
    }
}