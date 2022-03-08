using RichbetsResurrected.SharedKernel;

namespace RichbetsResurrected.Core.ProjectAggregate.Events;

public class ToDoItemCompletedEvent : BaseDomainEvent
{

    public ToDoItemCompletedEvent(ToDoItem completedItem)
    {
        CompletedItem = completedItem;
    }
    public ToDoItem CompletedItem { get; set; }
}