using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.Games.Crash;
using RichbetsResurrected.Interfaces.Client;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Games.Crash;
using RichbetsResurrected.Interfaces.Identity;
using RichbetsResurrected.Interfaces.Inventory;
using RichbetsResurrected.Utilities.Constants;
using SignalRSwaggerGen.Attributes;

namespace RichbetsResurrected.Communication.Crash.Hub;

[SignalRHub("/crashHub")]
[Authorize]
[SuppressMessage("ReSharper", "AsyncApostle.AsyncMethodNamingHighlighting")]
public class CrashHub : Hub<ICrashHub>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICrashService _crashService;
    private readonly IRichbetRepository _richbetRepository;
    private readonly IInventoryService _inventoryService;
    public CrashHub(IAccountRepository accountRepository, IRichbetRepository richbetRepository, ICrashService crashService, IInventoryService inventoryService)
    {
        _accountRepository = accountRepository;
        _richbetRepository = richbetRepository;
        _crashService = crashService;
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
        _crashService.GameState.AddOnlinePlayer(Context.ConnectionId, clientInfo);
        Console.WriteLine("Connected to crash hub");
        await base.OnConnectedAsync();
    }

    [SignalRHidden]
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _crashService.GameState.RemoveOnlinePlayer(Context.ConnectionId);
        Console.WriteLine("Disconnected from crash hub");
        return base.OnDisconnectedAsync(exception);
    }

    [SignalRMethod(summary: "Invokable by clients to join the crash")]
    public async Task<CrashJoinResult> JoinCrash(int amount)
    {
        var appUserId = Context.UserIdentifier;
        var discordId = Context.User.Claims.FirstOrDefault(c => c.Type == OAuthConstants.DiscordId).Value;
        var avatarUrl = _accountRepository.GetDiscordAvatarUrl(Context.User);

        var crashPlayer = new CrashPlayer
        {
            UserName = Context.User?.Identity.Name,
            DiscordUserId = discordId,
            IdentityUserId = Convert.ToInt32(appUserId),
            AvatarUrl = avatarUrl,
            Amount = amount
        };
        var result = await _crashService.JoinCrashAsync(crashPlayer);
        return result;
    }
    
    [SignalRMethod(summary: "Invoked when client want to cashout")]
    public async Task<CrashCashoutResult> Cashout(decimal? desiredMultiplier)
    {
        var appUserId = Convert.ToInt32(Context.UserIdentifier);
        if (desiredMultiplier != null)
        {
            desiredMultiplier = Math.Round(desiredMultiplier.Value, 2);
        }
        var result = await _crashService.CashoutAsync(appUserId, desiredMultiplier);
        return result;
    }

    [SignalRMethod(summary: "Stream for clients to receive the actual crash info")]
    public async IAsyncEnumerable<CrashInfo> StreamCrashInfo([EnumeratorCancellation][SignalRHidden] CancellationToken cancellationToken)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var crashInfo = _crashService.GameState.GetCrashInfo();
            yield return crashInfo;
            await Task.Delay(10, cancellationToken);
        }
    }

    // TODO: Add cashout method and autocashout endpoint
}