using Autofac;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Utils;

namespace RichbetsResurrected.Services.ScheduledTasks;

public class DailyResetScheduled : IScheduledTask
{
    private readonly ILifetimeScope _lifetimeScope;
    public DailyResetScheduled(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }
    
    public async Task ExecuteAsync()
    {
        // reset daily at the start of the day
        while (true)
        {
            var dateTime = DateTime.Now;
            if (dateTime.Hour == 0 && dateTime.Minute == 0)
            {
                var richbetRepo = _lifetimeScope.Resolve<IRichbetRepository>();
                var users = await richbetRepo.GetRichbetUsersAsync();
                foreach (var user in users)
                {
                    await richbetRepo.SetDailyToUserAsync(user.AppUserId, false);
                }
                // Daily reset is done, wait for the next day
                Console.WriteLine("Daily reset done");
                await Task.Delay(TimeSpan.FromHours(1));
            }
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}