using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.Roulette;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Identity;
using RichbetsResurrected.Utilities.Constants;
using SignalRSwaggerGen.Attributes;


namespace RichbetsResurrected.Communication.Client.Hub;

[SignalRHub("/rouletteHub")]
[Authorize]
[SuppressMessage("ReSharper", "AsyncApostle.AsyncMethodNamingHighlighting")]
public class ClientHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRichbetRepository _richbetRepository;
    public ClientHub(IAccountRepository accountRepository, IRichbetRepository richbetRepository)
    {
        _accountRepository = accountRepository;
        _richbetRepository = richbetRepository;

    }
    
    [SignalRMethod(summary: "Invokable by client to get currently logged in user info")]
    public async Task<ClientInfo> GetClientInfo()
    {
        var appUserId = Convert.ToInt32(Context.UserIdentifier);
        var discordId = Context.User.Claims.FirstOrDefault(c => c.Type == OAuthConstants.DiscordId).Value;
        var avatarUrl = _accountRepository.GetDiscordAvatarUrlAsync(Context.User);
        var richbetUser = await _richbetRepository.GetRichbetUserAsync(appUserId);
        var clientInfo = new ClientInfo()
        {
            UserName = Context.User?.Identity.Name,
            DiscordUserId = discordId,
            IdentityUserId = appUserId,
            AvatarUrl = avatarUrl,
            RichbetUser = richbetUser
        };
        return clientInfo;
    }
    
    [SignalRMethod(summary: "Invokable by client to get points for currently logged in user")]
    public async Task<int> GetPoints()
    {
        var appUserId = Convert.ToInt32(Context.UserIdentifier);
        var points = await _richbetRepository.GetPointsFromUserAsync(appUserId);
        return points;
    }
    
}