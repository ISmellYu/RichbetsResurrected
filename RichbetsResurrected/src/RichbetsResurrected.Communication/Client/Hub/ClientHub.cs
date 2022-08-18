using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using RichbetsResurrected.Entities.Client;
using RichbetsResurrected.Entities.DatabaseEntities.Statistics;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.DAL.Statistics;
using RichbetsResurrected.Interfaces.Identity;
using RichbetsResurrected.Utilities.Constants;
using SignalRSwaggerGen.Attributes;

namespace RichbetsResurrected.Communication.Client.Hub;

[SignalRHub("/clientHub")]
[Authorize]
[SuppressMessage("ReSharper", "AsyncApostle.AsyncMethodNamingHighlighting")]
public class ClientHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRichbetRepository _richbetRepository;
    private readonly IStatisticsRepository _statisticsRepository;
    public ClientHub(IAccountRepository accountRepository, IRichbetRepository richbetRepository, IStatisticsRepository statisticsRepository)
    {
        _accountRepository = accountRepository;
        _richbetRepository = richbetRepository;
        _statisticsRepository = statisticsRepository;
    }

    [SignalRMethod(summary: "Invokable by client to get currently logged in user info")]
    public async Task<ClientInfo> GetClientInfo()
    {
        var appUserId = Convert.ToInt32(Context.UserIdentifier);
        var discordId = Context.User.Claims.FirstOrDefault(c => c.Type == OAuthConstants.DiscordId).Value;
        var avatarUrl = _accountRepository.GetDiscordAvatarUrl(Context.User);
        var richbetUser = await _richbetRepository.GetRichbetUserAsync(appUserId);
        var stat = await _statisticsRepository.GetStatisticAsync(appUserId);
        var clientInfo = new ClientInfo
        {
            UserName = Context.User?.Identity.Name,
            DiscordUserId = discordId,
            IdentityUserId = appUserId,
            AvatarUrl = avatarUrl,
            RichbetUser = richbetUser,
            Statistic = stat
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