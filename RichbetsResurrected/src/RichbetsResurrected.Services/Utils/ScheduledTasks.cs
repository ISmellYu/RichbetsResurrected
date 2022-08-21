using Autofac;
using RichbetsResurrected.Interfaces.Utils;

namespace RichbetsResurrected.Services.Utils;

public class ScheduledTasks : IScheduledTasks
{
    private readonly ILifetimeScope _lifetimeScope;
    public ScheduledTasks(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }
    
    
}