using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RichbetsResurrected.Infrastructure.Data;
using RichbetsResurrected.Infrastructure.Data.Contexts;

namespace RichbetsResurrected.Infrastructure;

public static class StartupSetup
{
    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));
        // will be created in web project root
    }
}