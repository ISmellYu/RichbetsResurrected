using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace RichbetsResurrected.Identity.OAuth2Discord;

public static class DiscordEvents
{
    public static readonly string GuildId = "placeguildid";
    public static readonly string RoleId = "placeroleid";

    public static readonly string[] WhitelistedIds = { };

    public static async Task OnCreatingTicketAsync(OAuthCreatingTicketContext context)
    {
        var guilds = await GetGuildsAsync(context).ConfigureAwait(false);
        var userId = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
        if (!IsGuildMember(guilds, GuildId))
        {
            context.Fail($"User is not a member of the guild with id {GuildId}.");
            throw new DiscordAuthFailException($"Not in guild user id: {userId}");
        }


        // if (!IsWhitelisted(userId))
        // {
        //     context.Fail($"User is not whitelisted with id {userId}.");
        //     throw new DiscordAuthFailException($"Not whitelisted user id: {userId}");
        // }

        var userRoles = await GetDiscordRolesAsync(context).ConfigureAwait(false);
        if (!IsInRole(userRoles, RoleId))
        {
            context.Fail($"User is not in the role with id {RoleId}.");
            throw new DiscordAuthFailException($"Not in role user id: {userId}");
        }

        var tokens = context.Properties.GetTokens().ToList();

        tokens.Add(new AuthenticationToken
        {
            Name = "Ticket", Value = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
        });

        context.Properties.StoreTokens(tokens);
        // context.Properties.Items.Add("LoginProvider", "Discord");
        context.Success();
    }

    public static Task OnTicketReceivedAsync(TicketReceivedContext context)
    {
        return Task.CompletedTask;
    }

    private static async Task<IEnumerable<Guild>> GetGuildsAsync(OAuthCreatingTicketContext context)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/users/@me/guilds");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

        using var response =
            await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
                    context.HttpContext.RequestAborted)
                .ConfigureAwait(false);

        using var payload = await JsonDocument
            .ParseAsync(await response.Content.ReadAsStreamAsync(context.HttpContext.RequestAborted)
                .ConfigureAwait(false)).ConfigureAwait(false);

        var root = payload.RootElement.Clone();

        var guilds = root.EnumerateArray().Select(x => new Guild
        {
            Id = x.GetProperty("id").GetString() ?? string.Empty,
            Name = x.GetProperty("name").GetString() ?? string.Empty
        });
        return guilds;
    }

    private static bool IsGuildMember(IEnumerable<Guild> guilds, string guildId)
    {
        return guilds.Any(x => x.Id == guildId);
    }

    private static bool IsInRole(IEnumerable<DiscordRole> discordRoles, string roleId)
    {
        return discordRoles.Any(x => x.Id == roleId);
    }

    private static bool IsWhitelisted(string userId)
    {
        return WhitelistedIds.Contains(userId);
    }

    private static async Task<IEnumerable<DiscordRole>> GetDiscordRolesAsync(OAuthCreatingTicketContext context)
    {
        using var request =
            new HttpRequestMessage(HttpMethod.Get, $"https://discord.com/api/users/@me/guilds/{GuildId}/member");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

        using var response =
            await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
                    context.HttpContext.RequestAborted)
                .ConfigureAwait(false);

        using var payload = await JsonDocument
            .ParseAsync(await response.Content.ReadAsStreamAsync(context.HttpContext.RequestAborted)
                .ConfigureAwait(false)).ConfigureAwait(false);

        var root = payload.RootElement.Clone();

        var roles = root.GetProperty("roles").EnumerateArray().Select(id => new DiscordRole
        {
            Id = id.GetString() ?? string.Empty
        });
        return roles;
    }

    private class Guild
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    private class DiscordRole
    {
        public string Id { get; set; }
    }
}
