using System.Security.Claims;
using AspNet.Security.OAuth.Discord;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Core.Constants;
using RichbetsResurrected.Core.Interfaces.Games.Roulette;
using RichbetsResurrected.Core.Roulette.Entities;
using RichbetsResurrected.Core.Roulette.Hub;

namespace RichbetsResurrected.Infrastructure.Games.Roulette.Hub;

[Microsoft.AspNet.SignalR.Authorize]
public class RouletteHub : RouletteBaseHub
{
    private readonly IRouletteService _rouletteService;
    public RouletteHub(IRouletteService rouletteService)
    {
        _rouletteService = rouletteService;
    }

    [HubMethodName("RouletteHello")]
    public async Task<RouletteInfo> RouletteHelloAsync()
    {
        var rouletteInfo = await _rouletteService.GetRouletteInfoAsync();
        return rouletteInfo;
    }
    
    [HubMethodName("JoinRoulette")]
    public async Task<RouletteJoinResult> JoinRouletteAsync(int amount, RouletteColor color)
    {
        var appUserId = Context.UserIdentifier;
        var discordId = Context.User.Claims.FirstOrDefault(c => c.Type == Constants.DiscordId).Value;
        var avatarHash = Context.User.FindFirst(c => c.Type == DiscordAuthenticationConstants.Claims.AvatarHash)?.Value;
        var avatarUrl = $"https://cdn.discordapp.com/avatars/{discordId}/{avatarHash}.jpg";
        var roulettePlayer = new RoulettePlayer()
        {
            Amount = amount, Color = color, 
            UserName = Context.User?.Identity.Name,
            DiscordUserId = discordId,
            IdentityUserId = Convert.ToInt32(appUserId),
            AvatarUrl = avatarUrl
        };
        var joinResult = await _rouletteService.AddPlayerAsync(roulettePlayer);
        return joinResult;
    }
}
