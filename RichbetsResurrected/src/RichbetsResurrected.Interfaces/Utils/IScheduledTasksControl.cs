using Autofac;

namespace RichbetsResurrected.Interfaces.Utils;

public interface IScheduledTasksControl
{
    Task RunScheduledTasksAsync();
}