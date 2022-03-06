using RichbetsResurrected.Core.ProjectAggregate;
using RichbetsResurrected.SharedKernel;

namespace RichbetsResurrected.Core.ProjectAggregate.Events;

public class ToDoItemCompletedEvent : BaseDomainEvent
{
    public ToDoItem CompletedItem { get; set; }

    public ToDoItemCompletedEvent(ToDoItem completedItem)
    {
        CompletedItem = completedItem;
    }
}