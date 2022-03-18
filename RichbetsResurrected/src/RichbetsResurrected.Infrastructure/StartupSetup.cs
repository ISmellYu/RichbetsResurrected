using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RichbetsResurrected.Infrastructure.Data;
using RichbetsResurrected.Infrastructure.Data.Contexts;
using RichbetsResurrected.Infrastructure.Data.Identity.Models;
using RichbetsResurrected.Infrastructure.OAuth2Discord;

namespace RichbetsResurrected.Infrastructure;

public static class StartupSetup
{

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqliteConnection"); //Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext(connectionString);
        services.ConfigureCookies();
        services.AddIdentity();
        services.ConfigureAuthentication("806268020179206144", "FqArn7h-UjGGCTDcPG4ZqINa5YPF1mU0");
    }
    
    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));
        // will be created in web project root
    }

    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<AppDbContext>();
    }

    public static void ConfigureAuthentication(this IServiceCollection services, string discordClientId, string discordClientSecret)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = DiscordAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.Cookie.Name = "RichbetsResurrected";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.Cookie.IsEssential = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            
        }).AddDiscord(options =>
        {
            options.ClientId = discordClientId;
            options.ClientSecret = discordClientSecret;
            options.Scope.Add("guilds");
            options.Scope.Add("guilds.members.read");
            
            options.SaveTokens = true;

            options.Events.OnCreatingTicket = DiscordEvents.OnCreatingTicketAsync;

            options.Events.OnTicketReceived = DiscordEvents.OnTicketReceivedAsync;
        });;
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