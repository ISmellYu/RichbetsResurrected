namespace RichbetsResurrected.Core.Interfaces;

public interface IRichbetRepository
{
    Task CreateRichbetUserAsync(int identityUserId, string discordId);
}