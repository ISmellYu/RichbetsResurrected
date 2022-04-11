using System.Security.Claims;
using AspNet.Security.OAuth.Discord;
using MediatR;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Core.Constants;
using RichbetsResurrected.Core.Interfaces.Games.Roulette;
using RichbetsResurrected.Core.Roulette.Entities;
using RichbetsResurrected.Core.Roulette.Hub;
using RichbetsResurrected.Infrastructure.Identity.Interfaces;

namespace RichbetsResurrected.Infrastructure.Games.Roulette.Hub;

[Authorize]
public class RouletteHub : RouletteBaseHub
{
    private readonly IRouletteService _rouletteService;
    private readonly IAccountRepository _accountRepository;
    public RouletteHub(IRouletteService rouletteService, IAccountRepository accountRepository)
    {
        _rouletteService = rouletteService;
        _accountRepository = accountRepository;
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
        var avatarUrl = _accountRepository.GetDiscordAvatarUrlAsync(Context.User);
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
