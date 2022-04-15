using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Entities.Roulette;
using RichbetsResurrected.Interfaces.Games.Roulette;
using RichbetsResurrected.Interfaces.Identity;
using RichbetsResurrected.Utilities.Constants;
using SignalRSwaggerGen.Attributes;

namespace RichbetsResurrected.Communication.Roulette.Hub;

[SignalRHub("/rouletteHub")]
[Authorize]
[SuppressMessage("ReSharper", "AsyncApostle.AsyncMethodNamingHighlighting")]
public class RouletteHub : Hub<IRouletteHub>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRouletteService _rouletteService;
    public RouletteHub(IRouletteService rouletteService, IAccountRepository accountRepository)
    {
        _rouletteService = rouletteService;
        _accountRepository = accountRepository;
    }
    
    [SignalRMethod(summary: "Invokable by clients to get roulette info")]
    public async Task<RouletteInfo> RouletteHello()
    {
        var rouletteInfo = _rouletteService.GameState.GetRouletteInfoAsync();
        return rouletteInfo;
    }
    
    [SignalRMethod(summary: "Invokable by clients to join the roulette")]
    public async Task<RouletteJoinResult> JoinRoulette(int amount, RouletteColor color)
    {
        var appUserId = Context.UserIdentifier;
        var discordId = Context.User.Claims.FirstOrDefault(c => c.Type == OAuthConstants.DiscordId).Value;
        var avatarUrl = _accountRepository.GetDiscordAvatarUrlAsync(Context.User);
        var roulettePlayer = new RoulettePlayer
        {
            Amount = amount,
            Color = color,
            UserName = Context.User?.Identity.Name,
            DiscordUserId = discordId,
            IdentityUserId = Convert.ToInt32(appUserId),
            AvatarUrl = avatarUrl
        };
        var joinResult = await _rouletteService.AddPlayerAsync(roulettePlayer);
        return joinResult;
    }

    [SignalRMethod(summary: "Stream for clients to receive the actual roulette result")]
    public async IAsyncEnumerable<RouletteInfo> StreamRouletteInfo()
    {
        while (true)
        {
            var rouletteInfo = _rouletteService.GameState.GetRouletteInfoAsync();
            await Task.Delay(10);
            yield return rouletteInfo;
        }
    }
}