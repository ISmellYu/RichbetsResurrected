using MediatR;

namespace RichbetsResurrected.SharedKernel;

public abstract class BaseDomainEvent : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}