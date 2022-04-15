using RichbetsResurrected.Entities.DatabaseEntities;

namespace RichbetsResurrected.Interfaces.DAL;

public interface IRichbetRepository
{
    Task CreateRichbetUserAsync(int identityUserId, string discordId);
    Task DeleteRichbetUserByIdentityIdAsync(int identityUserId);
    Task AddPointsToUserAsync(int identityUserId, int points);
    Task RemovePointsFromUserAsync(int identityUserId, int points);
    Task SetDailyToUserAsync(int identityUserId, bool isRedeemed);
    Task<float> GetMultiplierFromUserAsync(int identityUserId);
    Task<int> GetPointsFromUserAsync(int identityUserId);
    Task<RichbetUser> GetRichbetUserAsync(int identityUserId);
 }