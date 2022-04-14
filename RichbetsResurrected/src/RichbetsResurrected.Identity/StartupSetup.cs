using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RichbetsResurrected.Entities.Identity.Models;
using RichbetsResurrected.Identity.Contexts;
using RichbetsResurrected.Identity.OAuth2Discord;

namespace RichbetsResurrected.Identity;

public static class StartupSetup
{

    public static void AddAuthStuff(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqliteConnection"); //Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext(connectionString);
        services.ConfigureCookies();
        services.AddIdentity();
        services.ConfigureAuthentication("***REMOVED***", "***REMOVED***");
    }

    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));
        // will be created in web project root
    }

    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options => { })
            .AddEntityFrameworkStores<AppDbContext>();
    }

    public static void ConfigureAuthentication(this IServiceCollection services, string discordClientId, string discordClientSecret)
    {
        services.AddAuthentication().AddDiscord(options =>
        {
            // options.ForwardSignIn = CookieAuthenticationDefaults.AuthenticationScheme;
            // options.ForwardSignOut = CookieAuthenticationDefaults.AuthenticationScheme;
            options.ClientId = discordClientId;
            options.ClientSecret = discordClientSecret;
            options.Scope.Add("guilds");
            options.Scope.Add("guilds.members.read");
            options.CallbackPath = new PathString("/Account/signin-discord");


            options.SaveTokens = true;

            options.Events.OnCreatingTicket = DiscordEvents.OnCreatingTicketAsync;

            options.Events.OnTicketReceived = DiscordEvents.OnTicketReceivedAsync;
        });
        ;
    }

    public static void ConfigureCookies(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.Lax;
        });
    }
}