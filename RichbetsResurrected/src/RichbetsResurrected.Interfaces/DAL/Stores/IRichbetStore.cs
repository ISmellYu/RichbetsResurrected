using RichbetsResurrected.Entities.DatabaseEntities;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

namespace RichbetsResurrected.Interfaces.DAL.Stores;

public interface IRichbetStore
{
    IQueryable<RichbetAppUser> RichbetAppUsers { get; }
    IQueryable<RichbetUser> RichbetUsers { get; }
    Task<RichbetUser> CreateRichbetUserAsync(RichbetUser richbetUser);
    Task<RichbetAppUser> CreateRichbetAppUserAsync(RichbetAppUser richbetAppUser);
    Task RemoveRichbetUserAsync(int richbetUserId);
    Task RemoveRichbetUserByDiscordIdAsync(string discordId);
    Task RemoveRichbetUserByAppUserIdAsync(int identityUserId);
    Task RemoveRichbetAppUserAsync(int identityUserId, int richbetUserId, string discordId);
    Task RemoveRichbetAppUserByDiscordIdAsync(string discordId);
    Task RemoveRichbetAppUserByAppUserIdAsync(int identityUserId);
    Task RemoveRichbetAppUserByRichbetUserIdAsync(int richbetUserId);
    Task<RichbetUser> GetRichbetUserByIdAsync(int id);
    Task<RichbetUser> GetRichbetUserByDiscordIdAsync(string discordId);
    Task<RichbetUser> GetRichbetUserByIdentityIdAsync(int identityUserId);
    Task<RichbetAppUser> GetRichbetAppUserByDiscordIdAsync(string discordId);
    Task<RichbetAppUser> GetRichbetAppUserByIdentityIdAsync(int identityUserId);
    Task<RichbetAppUser> GetRichbetAppUserByRichbetUserIdAsync(int richbetUserId);
    Task AddPointsToRichbetUserAsync(int richbetUserId, int points);
    Task RemovePointsFromRichbetUserAsync(int richbetUserId, int points);
    Task ResetPointsFromRichbetUserAsync(int richbetUserId);
    Task<float> GetMultiplierFromRichbetUserAsync(int richbetUserId);
    Task UpdateMultiplierToRichbetUserAsync(int richbetUserId, int multiplier);
    Task SetDailyToRichbetUserAsync(int richbetUserId, bool dailyRedeemed);
    Task SetDailyRedeemedToRichbetUserAsync(int richbetUserId);
    Task SetDailyNotRedeemedToRichbetUserAsync(int richbetUserId);
    Task<bool> CheckIfExistsRichbetUserAsync(int richbetUserId);
    Task<bool> CheckIfExistsRichbetUserByDiscordIdAsync(string discordId);
    Task<bool> CheckIfExistsRichbetUserByAppUserIdAsync(int appUserId);
    Task<bool> CheckIfExistsRichbetAppUserAsync(int richbetUserId);
    Task<bool> CheckIfExistsRichbetAppUserByDiscordIdAsync(string discordId);
    Task<bool> CheckIfExistsRichbetAppUserByAppUserIdAsync(int identityUserId);
}