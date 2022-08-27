using Microsoft.EntityFrameworkCore;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;
using RichbetsResurrected.Identity.Contexts;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.DAL.Statistics;

namespace RichbetsResurrected.Identity.Repositories;

public class RichbetRepository : IRichbetRepository
{
    private readonly AppDbContext _context;
    private readonly IStatisticsRepository _statisticsRepository;

    public RichbetRepository(AppDbContext context, IStatisticsRepository statisticsRepository)
    {
        _context = context;
        _statisticsRepository = statisticsRepository;
    }
    public IQueryable<RichbetAppUser> RichbetAppUsers => _context.RichbetAppUsers.AsNoTracking();
    // Readonly
    public IQueryable<RichbetUser> RichbetUsers => _context.RichbetUsers.AsNoTracking();

    public async Task CreateRichbetUserAsync(int identityUserId, string discordId)
    {
        var user = new RichbetUser
        {
            AppUserId = identityUserId, Multiplier = 1.0f, Points = 0, DailyRedeemed = false
        };

        var exists = await RichbetUsers.AnyAsync(r => r.AppUserId == identityUserId);
        if (exists)
            return;

        await _context.RichbetUsers.AddAsync(user);
        await _context.SaveChangesAsync();
        await _context.RichbetAppUsers.AddAsync(new RichbetAppUser
        {
            AppUserId = identityUserId, DiscordUserId = discordId
        });
        await _context.SaveChangesAsync();
        
        await _statisticsRepository.AddStatisticAsync(identityUserId);
    }

    public async Task DeleteRichbetUserByIdentityIdAsync(int identityUserId)
    {
        
        _context.RichbetUsers.Remove(new RichbetUser
        {
            AppUserId = identityUserId
        });
        //_context.RichbetAppUsers.Remove(richbetAppUser);

    }

    public async Task AddPointsToUserAsync(int identityUserId, int points)
    {
        var richbetUser = await _context.RichbetUsers.FindAsync(identityUserId);
        if (richbetUser == null)
            return;
        richbetUser.Points += points;
        await _context.SaveChangesAsync();
        await _statisticsRepository.AddToWonPointsAsync(identityUserId, points);
    }

    public async Task RemovePointsFromUserAsync(int identityUserId, int points)
    {
        var richbetUser = await _context.RichbetUsers.FindAsync(identityUserId);
        if (richbetUser == null)
            return;
        richbetUser.Points -= points;
        await _context.SaveChangesAsync();
        await _statisticsRepository.AddToLostPointsAsync(identityUserId, points);
    }

    public async Task SetDailyToUserAsync(int identityUserId, bool isRedeemed)
    {
        var richbetUser = await _context.RichbetUsers.FindAsync(identityUserId);
        if (richbetUser == null)
            return;
        richbetUser.DailyRedeemed = isRedeemed;
        await _context.SaveChangesAsync();
    }

    public async Task<float> GetMultiplierFromUserAsync(int identityUserId)
    {
        var richbetUser = await _context.RichbetUsers.FindAsync(identityUserId);
        if (richbetUser == null)
            return 1.0f;
        var multiplier = richbetUser.Multiplier;
        return multiplier;
    }

    public async Task<int> GetPointsFromUserAsync(int identityUserId)
    {
        var richbetUser = await _context.RichbetUsers.FindAsync(identityUserId);
        if (richbetUser == null)
            return 0;
        var points = richbetUser.Points;
        return points;
    }

    public async Task<RichbetUser> GetRichbetUserAsync(int identityUserId)
    {
        var richbetUser = await RichbetUsers.FirstOrDefaultAsync(r => r.AppUserId == identityUserId);
        return richbetUser;
    }
    public async Task<List<RichbetUser>> GetRichbetUsersAsync()
    {
        var richbetUsers = await RichbetUsers.ToListAsync();
        return richbetUsers;
    }
}