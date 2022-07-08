using System.Security.Claims;
using System.Text.Encodings.Web;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RichbetsResurrected.Identity.OAuth2Discord;

public class DiscordAuthenticationHandlerNew : DiscordAuthenticationHandler
{
    // Class created to override the default HandleRemoteAuthenticateAsync because even when calling context.Fail() it doesnt do anything.

    public DiscordAuthenticationHandlerNew(IOptionsMonitor<DiscordAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
        
    }

    protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
    {
        try
        {
            return await base.HandleRemoteAuthenticateAsync();
        }
        catch (DiscordAuthFailException ex)
        {
            return HandleRequestResult.Fail(ex);
        }
    }
}