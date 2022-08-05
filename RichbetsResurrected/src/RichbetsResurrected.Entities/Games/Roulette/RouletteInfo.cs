﻿using RichbetsResurrected.Entities.Client;

namespace RichbetsResurrected.Entities.Games.Roulette;

public class RouletteInfo
{
    public List<RoulettePlayer> Players { get; set; }
    public List<RouletteResult> Results { get; set; }
    public List<ClientInfo> OnlinePlayers { get; set; }
    public bool AllowBetting { get; set; }
    public bool IsRolling { get; set; }
    public decimal TimeLeft { get; set; }
}