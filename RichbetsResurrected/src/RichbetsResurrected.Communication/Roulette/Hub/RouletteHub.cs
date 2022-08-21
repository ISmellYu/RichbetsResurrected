using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.Games.Roulette;
using RichbetsResurrected.Interfaces.Client;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Games.Roulette;
using RichbetsResurrected.Interfaces.Identity;
using RichbetsResurrected.Interfaces.Inventory;
using RichbetsResurrected.Interfaces.Shop;
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
    private readonly IInventoryService _inventoryService;
    public RouletteHub(IRouletteService rouletteService, IAccountRepository accountRepository, IRichbetRepository richbetRepository, IInventoryService inventoryService)
    {
        _rouletteService = rouletteService;
        _accountRepository = accountRepository;
        _richbetRepository = richbetRepository;
        _inventoryService = inventoryService;
    }

    [SignalRHidden]
    public override async Task OnConnectedAsync()
    {
        var appUserId = Convert.ToInt32(Context.UserIdentifier);
        var discordId = Context.User.Claims.FirstOrDefault(c => c.Type == OAuthConstants.DiscordId).Value;
        var avatarUrl = _accountRepository.GetDiscordAvatarUrl(Context.User);
        var richbetUser = await _richbetRepository.GetRichbetUserAsync(appUserId);
        var inv = _inventoryService.GetInventory(appUserId);
        var clientInfo = new ClientInfo
        {
            UserName = Context.User?.Identity.Name,
            DiscordUserId = discordId,
            IdentityUserId = appUserId,
            AvatarUrl = avatarUrl,
            RichbetUser = richbetUser,
            EquippedItems = inv.EquippedItems
        };
        _rouletteService.GameState.AddOnlinePlayer(Context.ConnectionId, clientInfo);
        Console.WriteLine("Connected to roulette hub");
        await base.OnConnectedAsync();
    }

    [SignalRHidden]
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
    public async IAsyncEnumerable<RouletteInfo> StreamRouletteInfo([EnumeratorCancellation][SignalRHidden] CancellationToken cancellationToken)
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