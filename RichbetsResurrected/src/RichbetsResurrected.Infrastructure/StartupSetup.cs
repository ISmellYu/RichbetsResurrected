using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RichbetsResurrected.Infrastructure.Data;
using RichbetsResurrected.Infrastructure.Data.Contexts;
using RichbetsResurrected.Infrastructure.Data.Identity.Models;

namespace RichbetsResurrected.Infrastructure;

public static class StartupSetup
{

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqliteConnection"); //Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext(connectionString);
        services.AddIdentity();
        services.ConfigureAuthentication();
        services.AddDiscordOAuth("806268020179206144", "FqArn7h-UjGGCTDcPG4ZqINa5YPF1mU0");
    }
    
    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));
        // will be created in web project root
    }
    
    public static void AddDiscordOAuth(this IServiceCollection services, string clientId, string clientSecret)
    {
        services.AddAuthentication().AddDiscord(options =>
        {
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.Scope.Add("guilds");
            options.Scope.Add("identify");
            
            options.SaveTokens = true;
            
            options.Events.OnCreatingTicket = context =>
            {
                var tokens = context.Properties.GetTokens().ToList();
                
                tokens.Add(new AuthenticationToken
                {
                    Name = "Ticket",
                    Value = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
                });
                
                context.Properties.StoreTokens(tokens);
                return Task.CompletedTask;
            };
        });
    }
    
    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<UserAppContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
        });
    }
}