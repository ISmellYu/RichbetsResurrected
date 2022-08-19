using Microsoft.EntityFrameworkCore;
using RichbetsResurrected.Entities.DatabaseEntities.Statistics;
using RichbetsResurrected.Identity.Contexts;
using RichbetsResurrected.Interfaces.DAL.Statistics;

namespace RichbetsResurrected.Identity.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly AppDbContext _context;

    public DbSet<Statistic> Statistics => _context.Statistics;

    public StatisticsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Statistic> GetStatisticAsync(int userId)
    {
        if (!await DoesStatisticsExistAsync(userId))
            await AddStatisticAsync(userId);
        var stat = await Statistics.AsNoTracking().FirstAsync(p => p.RichetUserId == userId);
        return stat;
    }
    public async Task AddStatisticAsync(int userId)
    {
        if (await DoesStatisticsExistAsync(userId))
            return;
        var stat = new Statistic
        {
            LoseAmount = 0, WinAmount = 0, RichetUserId = userId
        };
        
        await _context.Statistics.AddAsync(stat);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> DoesStatisticsExistAsync(int userId)
    {
        return await Statistics.FindAsync(userId) != null;
    }
    
    public async Task AddToWonPointsAsync(int userId, int points)
    {
        if (!await DoesStatisticsExistAsync(userId))
        {
            await AddStatisticAsync(userId);
        }
        
        var stat = await Statistics.FindAsync(userId);
        if (stat == null)
            return;
        stat.WinAmount += points;
        await _context.SaveChangesAsync();
    }
    public async Task AddToLostPointsAsync(int userId, int points)
    {
        if (!await DoesStatisticsExistAsync(userId))
        {
            await AddStatisticAsync(userId);
        }
        
        var stat = await Statistics.FindAsync(userId);
        if (stat == null)
            return;
        stat.LoseAmount += points;
        await _context.SaveChangesAsync();
    }
    public Task<int> GetGlobalWinAmountAsync()
    {
        var stats = Statistics.AsNoTracking();
        return stats.SumAsync(p => p.WinAmount);
    }
    public Task<int> GetGlobalLostAmountAsync()
    {
        var stats = Statistics.AsNoTracking();
        return stats.SumAsync(p => p.LoseAmount);
    }
}