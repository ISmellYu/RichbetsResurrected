namespace RichbetsResurrected.SharedKernel;

// This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
public abstract class BaseEntity
{

    public List<BaseDomainEvent> Events = new();
    public int Id { get; set; }
}