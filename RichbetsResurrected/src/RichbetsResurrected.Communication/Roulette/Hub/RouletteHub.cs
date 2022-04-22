using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.Roulette;
using RichbetsResurrected.Interfaces.DAL;
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
    private readonly IRichbetRepository _richbetRepository;
    private readonly IRouletteService _rouletteService;
    public RouletteHub(IRouletteService rouletteService, IAccountRepository accountRepository, IRichbetRepository richbetRepository)
    {
        _rouletteService = rouletteService;
        _accountRepository = accountRepository;
        _richbetRepository = richbetRepository;
    }

    public override async Task OnConnectedAsync()
    {
        var appUserId = Convert.ToInt32(Context.UserIdentifier);
        var discordId = Context.User.Claims.FirstOrDefault(c => c.Type == OAuthConstants.DiscordId).Value;
        var avatarUrl = _accountRepository.GetDiscordAvatarUrl(Context.User);
        var richbetUser = await _richbetRepository.GetRichbetUserAsync(appUserId);
        var clientInfo = new ClientInfo
        {
            UserName = Context.User?.Identity.Name,
            DiscordUserId = discordId,
            IdentityUserId = appUserId,
            AvatarUrl = avatarUrl,
            RichbetUser = richbetUser
        };
        _rouletteService.GameState.AddOnlinePlayer(Context.ConnectionId, clientInfo);
        Console.WriteLine("Connected to roulette hub");
        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _rouletteService.GameState.RemoveOnlinePlayer(Context.ConnectionId);
        Console.WriteLine("Disconnected from roulette hub");
        return base.OnDisconnectedAsync(exception);
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
        var avatarUrl = _accountRepository.GetDiscordAvatarUrl(Context.User);
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

    [SignalRMethod(summary: "Stream for clients to receive the actual roulette info")]
    public async IAsyncEnumerable<RouletteInfo> StreamRouletteInfo([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var rouletteInfo = _rouletteService.GameState.GetRouletteInfoAsync();
            yield return rouletteInfo;
            await Task.Delay(10, cancellationToken);
        }
    }
}