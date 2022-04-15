using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RichbetsResurrected.Entities.DatabaseEntities;
using RichbetsResurrected.Identity.Contexts;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.DAL.Stores;

namespace RichbetsResurrected.Identity.BaseRichbet;

public class RichbetRepository : IRichbetRepository
{
    private readonly AppDbContext _context;
    public int KlantId { get; set; }
    public IQueryable<RichbetAppUser> RichbetAppUsers => _context.RichbetAppUsers.AsNoTracking();
    // Readonly
    public IQueryable<RichbetUser> RichbetUsers => _context.RichbetUsers.AsNoTracking();

    public RichbetRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateRichbetUserAsync(int identityUserId, string discordId)
    {
        var user = new RichbetUser
        {
            Multiplier = 1.0f, Points = 0, DailyRedeemed = false
        };

        var exists = await RichbetAppUsers.AnyAsync(r => r.AppUserId == identityUserId);
        if (exists)
            return;
        
        await _context.RichbetUsers.AddAsync(user);
        await _context.RichbetAppUsers.AddAsync(new RichbetAppUser()
        {
            AppUserId = identityUserId, DiscordUserId = discordId,
            RichbetUserId = user.Id
        });
    }

    public async Task DeleteRichbetUserByIdentityIdAsync(int identityUserId)
    {
        var richbetAppUser = await RichbetAppUsers.FirstOrDefaultAsync(r => r.AppUserId == identityUserId);
        if (richbetAppUser == null)
            return;

        
        _context.RichbetUsers.Remove(new RichbetUser() { Id = richbetAppUser.RichbetUserId });
        _context.RichbetAppUsers.Remove(richbetAppUser);

    }

    public async Task AddPointsToUserAsync(int identityUserId, int points)
    {
        var richbetAppUser = await RichbetAppUsers.FirstOrDefaultAsync(r => r.AppUserId == identityUserId);
        if (richbetAppUser == null)
            return;
        var richbetUser = await _context.RichbetUsers.FindAsync(richbetAppUser.RichbetUserId);
        Console.WriteLine($"Add: {KlantId}");
        richbetUser.Points += points;
        await _context.SaveChangesAsync();
        KlantId++;
    }

    public async Task RemovePointsFromUserAsync(int identityUserId, int points)
    {
        var richbetAppUser = await RichbetAppUsers.FirstOrDefaultAsync(r => r.AppUserId == identityUserId);
        if (richbetAppUser == null)
            return;
        var richbetUser = await _context.RichbetUsers.FindAsync(richbetAppUser.RichbetUserId);
        Console.WriteLine($"Remove: {KlantId}");
        richbetUser.Points -= points;
        await _context.SaveChangesAsync();
        KlantId++;
    }

    public async Task SetDailyToUserAsync(int identityUserId, bool isRedeemed)
    {
        var richbetAppUser = await RichbetAppUsers.FirstOrDefaultAsync(r => r.AppUserId == identityUserId);
        if (richbetAppUser == null)
            return;
        var richbetUser = await _context.RichbetUsers.FindAsync(richbetAppUser.RichbetUserId);
        richbetUser.DailyRedeemed = isRedeemed;
        await _context.SaveChangesAsync();
    }

    public async Task<float> GetMultiplierFromUserAsync(int identityUserId)
    {
        var richbetAppUser = await RichbetAppUsers.FirstOrDefaultAsync(r => r.AppUserId == identityUserId);
        if (richbetAppUser == null)
            return -1;
        var richbetUser = await _context.RichbetUsers.FirstOrDefaultAsync(r => r.Id == richbetAppUser.RichbetUserId);
        var multiplier = richbetUser.Multiplier;
        return multiplier;
    }

    public async Task<int> GetPointsFromUserAsync(int identityUserId)
    {
        var richbetAppUser = await RichbetAppUsers.FirstOrDefaultAsync(r => r.AppUserId == identityUserId);
        if (richbetAppUser == null)
            return -1;
        var richbetUser = await _context.RichbetUsers.FirstOrDefaultAsync(r => r.Id == richbetAppUser.RichbetUserId);
        var points = richbetUser.Points;
        return points;
    }

    public async Task<RichbetUser> GetRichbetUserAsync(int identityUserId)
    {
        var richbetAppUser = await RichbetAppUsers.FirstOrDefaultAsync(r => r.AppUserId == identityUserId);
        if (richbetAppUser == null)
            return null;
        var richbetUser = await _context.RichbetUsers.FirstOrDefaultAsync(r => r.Id == richbetAppUser.RichbetUserId);
        return richbetUser;
    }
}