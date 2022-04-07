using RichbetsResurrected.Core.DiscordAggregate;
using RichbetsResurrected.Core.Interfaces;
using RichbetsResurrected.Core.Interfaces.Stores;

namespace RichbetsResurrected.Infrastructure.BaseRichbet;

public class RichbetRepository : IRichbetRepository
{
    private readonly IRichbetStore _store;
    
    public RichbetRepository(IRichbetStore store)
    {
        _store = store;
    }

    public async Task CreateRichbetUserAsync(int identityUserId, string discordId)
    {
        var user = new RichbetUser()
        {
            Multiplier = 1.0f, Points = 0, DailyRedeemed = false
        };

        var exists = await _store.CheckIfExistsRichbetUserByAppUserIdAsync(identityUserId);
        if (exists)
            return;
        
        await _store.CreateRichbetUserAsync(user);
        await _store.CreateRichbetAppUserAsync(new RichbetAppUser()
        {
            AppUserId = identityUserId,
            RichbetUserId = user.Id,
            DiscordUserId = discordId
        });
    }

    public async Task DeleteRichbetUserByIdentityIdAsync(int identityUserId)
    {
        var exists = await _store.CheckIfExistsRichbetUserByAppUserIdAsync(identityUserId);
        if (!exists)
            return;
        
        await _store.RemoveRichbetUserByAppUserIdAsync(identityUserId);

        exists = await _store.CheckIfExistsRichbetAppUserByAppUserIdAsync(identityUserId);
        if (!exists)
            return;

        await _store.RemoveRichbetAppUserByAppUserIdAsync(identityUserId);
    }

    public async Task AddPointsToUserAsync(int identityUserId, int points)
    {
        var exists = await _store.CheckIfExistsRichbetUserByAppUserIdAsync(identityUserId);
        if (!exists)
            return;
        var richbetUser = await _store.GetRichbetUserByIdentityIdAsync(identityUserId);
        await _store.AddPointsToRichbetUserAsync(richbetUser.Id, points);
    }
    
    public async Task RemovePointsFromUserAsync(int identityUserId, int points)
    {
        var exists = await _store.CheckIfExistsRichbetUserByAppUserIdAsync(identityUserId);
        if (!exists)
            return;
        var richbetUser = await _store.GetRichbetUserByIdentityIdAsync(identityUserId);
        await _store.RemovePointsFromRichbetUserAsync(richbetUser.Id, points);
    }
    
    public async Task SetDailyToUserAsync(int identityUserId, bool isRedeemed)
    {
        var exists = await _store.CheckIfExistsRichbetUserByAppUserIdAsync(identityUserId);
        if (!exists)
            return;
        var richbetUser = await _store.GetRichbetUserByIdentityIdAsync(identityUserId);
        await _store.SetDailyToRichbetUserAsync(richbetUser.Id, isRedeemed);
    }

    public async Task<float?> GetMultiplierFromUserAsync(int identityUserId)
    {
        var exists = await _store.CheckIfExistsRichbetUserByAppUserIdAsync(identityUserId);
        if (!exists)
            return null;
        var richbetUser = await _store.GetRichbetUserByIdentityIdAsync(identityUserId);
        var multiplier = richbetUser.Multiplier;
        return multiplier;
    }
}