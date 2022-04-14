using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RichbetsResurrected.Entities.DatabaseEntities;
using RichbetsResurrected.Identity.Contexts;
using RichbetsResurrected.Interfaces.DAL.Stores;

namespace RichbetsResurrected.Identity.BaseRichbet.Stores;

public class RichbetStore : IRichbetStore
{
    // Write and read operations
    private readonly AppDbContext _context;
    public RichbetStore(AppDbContext context)
    {
        _context = context;
    }
    // Readonly
    public IQueryable<RichbetAppUser> RichbetAppUsers => _context.RichbetAppUsers.AsNoTracking();
    // Readonly
    public IQueryable<RichbetUser> RichbetUsers => _context.RichbetUsers.AsNoTracking();

    public async Task<RichbetUser> CreateRichbetUserAsync(RichbetUser richbetUser)
    {
        var userFromDb = await _context.RichbetUsers.AddAsync(richbetUser);
        await _context.SaveChangesAsync();
        userFromDb.State = EntityState.Detached;
        return userFromDb.Entity;
    }

    public async Task<RichbetAppUser> CreateRichbetAppUserAsync(RichbetAppUser richbetAppUser)
    {
        var userFromDb = await _context.RichbetAppUsers.AddAsync(richbetAppUser);
        await _context.SaveChangesAsync();
        userFromDb.State = EntityState.Detached;
        return userFromDb.Entity;
    }
    public Task RemoveRichbetUserAsync(int richbetUserId)
    {
        _context.RichbetUsers.Remove(new RichbetUser
        {
            Id = richbetUserId
        });
        return _context.SaveChangesAsync();
    }
    public async Task RemoveRichbetUserByDiscordIdAsync(string discordId)
    {
        var user = await GetRichbetUserByDiscordIdAsync(discordId);
        _context.RichbetUsers.Remove(user);
    }
    public async Task RemoveRichbetUserByAppUserIdAsync(int identityUserId)
    {
        var user = await GetRichbetUserByIdentityIdAsync(identityUserId);
        _context.RichbetUsers.Remove(user);
    }
    public Task RemoveRichbetAppUserAsync(int identityUserId, int richbetUserId, string discordId)
    {
        _context.RichbetAppUsers.Remove(new RichbetAppUser
        {
            AppUserId = identityUserId, DiscordUserId = discordId, RichbetUserId = richbetUserId
        });
        return _context.SaveChangesAsync();
    }
    public async Task RemoveRichbetAppUserByDiscordIdAsync(string discordId)
    {
        var richbetAppUser = await GetRichbetAppUserByDiscordIdAsync(discordId);
        _context.RichbetAppUsers.Remove(richbetAppUser);
        await _context.SaveChangesAsync();
    }
    public async Task RemoveRichbetAppUserByAppUserIdAsync(int identityUserId)
    {
        var richbetAppUser = await GetRichbetAppUserByIdentityIdAsync(identityUserId);
        _context.RichbetAppUsers.Remove(richbetAppUser);
        await _context.SaveChangesAsync();
    }
    public async Task RemoveRichbetAppUserByRichbetUserIdAsync(int richbetUserId)
    {
        var richbetAppUser = await GetRichbetAppUserByRichbetUserIdAsync(richbetUserId);
        _context.RichbetAppUsers.Remove(richbetAppUser);
        await _context.SaveChangesAsync();
    }

    public async Task<RichbetUser> GetRichbetUserByIdAsync(int id)
    {
        var user = await RichbetUsers.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<RichbetUser> GetRichbetUserByDiscordIdAsync(string discordId)
    {
        var richbetAppUsers = await GetRichbetAppUserByDiscordIdAsync(discordId);
        var user = await GetRichbetUserByIdAsync(richbetAppUsers.RichbetUserId);
        return user;
    }

    public async Task<RichbetUser> GetRichbetUserByIdentityIdAsync(int identityUserId)
    {
        var richbetAppUsers = await GetRichbetAppUserByIdentityIdAsync(identityUserId);
        var user = await GetRichbetUserByIdAsync(richbetAppUsers.RichbetUserId);
        return user;
    }

    public async Task<RichbetAppUser> GetRichbetAppUserByDiscordIdAsync(string discordId)
    {
        var user = await RichbetAppUsers.FirstOrDefaultAsync(u => u.DiscordUserId == discordId);
        return user;
    }

    public async Task<RichbetAppUser> GetRichbetAppUserByIdentityIdAsync(int identityUserId)
    {
        var user = await RichbetAppUsers.FirstOrDefaultAsync(u => u.AppUserId == identityUserId);
        return user;
    }

    public async Task<RichbetAppUser> GetRichbetAppUserByRichbetUserIdAsync(int richbetUserId)
    {
        var user = await RichbetAppUsers.FirstOrDefaultAsync(u => u.RichbetUserId == richbetUserId);
        return user;
    }

    public async Task AddPointsToRichbetUserAsync(int richbetUserId, int points)
    {
        var richbetUser = await GetRichbetUserByIdAsync(richbetUserId);
        richbetUser.Points += points;
        _context.RichbetUsers.Update(richbetUser);
        await _context.SaveChangesAsync();
    }

    public async Task RemovePointsFromRichbetUserAsync(int richbetUserId, int points)
    {
        var richbetUser = await GetRichbetUserByIdAsync(richbetUserId);
        richbetUser.Points -= points;
        _context.RichbetUsers.Update(richbetUser);
        await _context.SaveChangesAsync();
    }

    public async Task ResetPointsFromRichbetUserAsync(int richbetUserId)
    {
        var richbetUser = await GetRichbetUserByIdAsync(richbetUserId);
        richbetUser.Points = 0;
        _context.RichbetUsers.Update(richbetUser);
        await _context.SaveChangesAsync();
    }

    public async Task<float> GetMultiplierFromRichbetUserAsync(int richbetUserId)
    {
        var richbetUser = await GetRichbetUserByIdAsync(richbetUserId);
        return richbetUser.Multiplier;
    }
    public async Task UpdateMultiplierToRichbetUserAsync(int richbetUserId, int multiplier)
    {
        var richbetUser = await GetRichbetUserByIdAsync(richbetUserId);
        richbetUser.Multiplier = multiplier;
        _context.RichbetUsers.Update(richbetUser);
        await _context.SaveChangesAsync();
    }

    public async Task SetDailyToRichbetUserAsync(int richbetUserId, bool dailyRedeemed)
    {
        var richbetUser = await GetRichbetUserByIdAsync(richbetUserId);
        richbetUser.DailyRedeemed = dailyRedeemed;
        _context.RichbetUsers.Update(richbetUser);
        await _context.SaveChangesAsync();
    }

    public Task SetDailyRedeemedToRichbetUserAsync(int richbetUserId)
    {
        return SetDailyToRichbetUserAsync(richbetUserId, true);
    }

    public Task SetDailyNotRedeemedToRichbetUserAsync(int richbetUserId)
    {
        return SetDailyToRichbetUserAsync(richbetUserId, false);
    }

    public Task<bool> CheckIfExistsRichbetUserAsync(int richbetUserId)
    {
        return CheckIfExistsRichbetAppUserAsync(richbetUserId);
    }

    public Task<bool> CheckIfExistsRichbetUserByDiscordIdAsync(string discordId)
    {
        // Using foreign key to check if exists
        return CheckIfExistsRichbetAppUserByDiscordIdAsync(discordId);
    }

    public Task<bool> CheckIfExistsRichbetUserByAppUserIdAsync(int appUserId)
    {
        // Using foreign key to check if exists
        return CheckIfExistsRichbetAppUserByAppUserIdAsync(appUserId);
    }

    public Task<bool> CheckIfExistsRichbetAppUserAsync(int richbetUserId)
    {
        return RichbetAppUsers.AnyAsync(u => u.RichbetUserId == richbetUserId);
    }

    public Task<bool> CheckIfExistsRichbetAppUserByDiscordIdAsync(string discordId)
    {
        return RichbetAppUsers.AnyAsync(u => u.DiscordUserId == discordId);
    }

    public Task<bool> CheckIfExistsRichbetAppUserByAppUserIdAsync(int identityUserId)
    {
        return RichbetAppUsers.AnyAsync(u => u.AppUserId == identityUserId);
    }
}