using RichbetsResurrected.Entities.DatabaseEntities;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

namespace RichbetsResurrected.Interfaces.DAL.Stores;

public interface IRichbetStore
{
    IQueryable<RichbetAppUser> RichbetAppUsers { get; }
    IQueryable<RichbetUser> RichbetUsers { get; }
    Task<RichbetUser> CreateRichbetUserAsync(RichbetUser richbetUser);
    Task<RichbetAppUser> CreateRichbetAppUserAsync(RichbetAppUser richbetAppUser);
    Task RemoveRichbetUserAsync(int identityUserId);
    Task RemoveRichbetUserByDiscordIdAsync(string discordId);
    Task RemoveRichbetUserByAppUserIdAsync(int identityUserId);
    Task RemoveRichbetAppUserAsync(int identityUserId, string discordId);
    Task RemoveRichbetAppUserByDiscordIdAsync(string discordId);
    Task RemoveRichbetAppUserByAppUserIdAsync(int identityUserId);
    Task RemoveRichbetAppUserByRichbetUserIdAsync(int identityUserId);
    Task<RichbetUser> GetRichbetUserByIdAsync(int id);
    Task<RichbetUser> GetRichbetUserByDiscordIdAsync(string discordId);
    Task<RichbetUser> GetRichbetUserByIdentityIdAsync(int identityUserId);
    Task<RichbetAppUser> GetRichbetAppUserByDiscordIdAsync(string discordId);
    Task<RichbetAppUser> GetRichbetAppUserByIdentityIdAsync(int identityUserId);
    Task<RichbetAppUser> GetRichbetAppUserByRichbetUserIdAsync(int identityUserId);
    Task AddPointsToRichbetUserAsync(int identityUserId, int points);
    Task RemovePointsFromRichbetUserAsync(int identityUserId, int points);
    Task ResetPointsFromRichbetUserAsync(int identityUserId);
    Task<float> GetMultiplierFromRichbetUserAsync(int identityUserId);
    Task UpdateMultiplierToRichbetUserAsync(int identityUserId, int multiplier);
    Task SetDailyToRichbetUserAsync(int identityUserId, bool dailyRedeemed);
    Task SetDailyRedeemedToRichbetUserAsync(int identityUserId);
    Task SetDailyNotRedeemedToRichbetUserAsync(int identityUserId);
    Task<bool> CheckIfExistsRichbetUserAsync(int identityUserId);
    Task<bool> CheckIfExistsRichbetUserByDiscordIdAsync(string discordId);
    Task<bool> CheckIfExistsRichbetUserByAppUserIdAsync(int appUserId);
    Task<bool> CheckIfExistsRichbetAppUserAsync(int identityUserId);
    Task<bool> CheckIfExistsRichbetAppUserByDiscordIdAsync(string discordId);
    Task<bool> CheckIfExistsRichbetAppUserByAppUserIdAsync(int identityUserId);
}