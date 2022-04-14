using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Entities.Roulette;
using RichbetsResurrected.Interfaces.Identity;
using RichbetsResurrected.Interfaces.Interfaces.Games.Roulette;
using RichbetsResurrected.Utilities.Constants;

namespace RichbetsResurrected.Communication.Roulette.Hub;

[Authorize]
public class RouletteHub : Hub<IRouletteHub>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRouletteService _rouletteService;
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
}