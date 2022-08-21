using Autofac;
using RichbetsResurrected.Interfaces.Utils;

namespace RichbetsResurrected.Services.Utils;

public class ScheduledTasksControl : IScheduledTasksControl
{
    private readonly ILifetimeScope _lifetimeScope;
    public ScheduledTasksControl(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }
    
    public Task RunScheduledTasksAsync()
    {
        return Task.CompletedTask;
    }
}