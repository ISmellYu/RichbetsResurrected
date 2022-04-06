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

}