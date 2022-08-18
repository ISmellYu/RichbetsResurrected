using RichbetsResurrected.Entities.DatabaseEntities.Statistics;

namespace RichbetsResurrected.Interfaces.DAL.Statistics;

public interface IStatisticsRepository
{
    Task<Statistic> GetStatisticAsync(int userId);
    Task AddStatisticAsync(int userId);
    Task<bool> DoesStatisticsExistAsync(int userId);
    Task AddToWonPointsAsync(int userId, int points);
    Task AddToLostPointsAsync(int userId, int points);
    Task<int> GetGlobalWinAmountAsync();
    Task<int> GetGlobalLostAmountAsync();
}