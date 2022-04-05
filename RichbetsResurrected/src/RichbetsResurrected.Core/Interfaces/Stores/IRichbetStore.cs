using RichbetsResurrected.Core.DiscordAggregate;

namespace RichbetsResurrected.Core.Interfaces.Stores;

public interface IRichbetStore
{
    IQueryable<RichbetAppUser> RichbetAppUsers { get; }
    IQueryable<RichbetUser> RichbetUsers { get; }
    Task<RichbetUser> CreateRichbetUserAsync(RichbetUser richbetUser);
    Task<RichbetAppUser> CreateRichbetAppUserAsync(RichbetAppUser richbetAppUser);
    Task RemoveRichbetUserAsync(int richbetUserId);
    Task RemoveRichbetUserByDiscordIdAsync(string discordId);
    Task RemoveRichbetUserByAppUserIdAsync(int appUserId);
    Task RemoveRichbetAppUserAsync(int appUserId, int richbetUserId, string discordId);
    Task RemoveRichbetAppUserByDiscordIdAsync(string discordId);
    Task RemoveRichbetAppUserByAppUserIdAsync(int appUserId);
    Task RemoveRichbetAppUserByRichbetUserIdAsync(int richbetUserId);
    Task<RichbetUser> GetRichbetUserByIdAsync(int id);
    Task<RichbetUser> GetRichbetUserByDiscordIdAsync(string discordId);
    Task<RichbetUser> GetRichbetUserByIdentityIdAsync(int appUserId);
    Task<RichbetAppUser> GetRichbetAppUserByDiscordIdAsync(string discordId);
    Task<RichbetAppUser> GetRichbetAppUserByIdentityIdAsync(int appUserId);
    Task<RichbetAppUser> GetRichbetAppUserByRichbetUserIdAsync(int richbetUserId);
    Task AddPointsToRichbetUserAsync(int richbetUserId, int points);
    Task RemovePointsFromRichbetUserAsync(int richbetUserId, int points);
    Task ResetPointsFromRichbetUserAsync(int richbetUserId);
    Task UpdateMultiplierToRichbetUserAsync(int richbetUserId, int multiplier);
    Task SetDailyToRichbetUserAsync(int richbetUserId, bool dailyRedeemed);
    Task SetDailyRedeemedToRichbetUserAsync(int richbetUserId);
    Task SetDailyNotRedeemedToRichbetUserAsync(int richbetUserId);
    Task<bool> CheckIfExistsRichbetUserAsync(int richbetUserId);
    Task<bool> CheckIfExistsRichbetUserByDiscordIdAsync(string discordId);
    Task<bool> CheckIfExistsRichbetUserByAppUserIdAsync(int appUserId);
    Task<bool> CheckIfExistsRichbetAppUserAsync(int richbetUserId);
    Task<bool> CheckIfExistsRichbetAppUserByDiscordIdAsync(string discordId);
    Task<bool> CheckIfExistsRichbetAppUserByAppUserIdAsync(int appUserId);
}