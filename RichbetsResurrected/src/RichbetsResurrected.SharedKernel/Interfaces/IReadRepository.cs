using Ardalis.Specification;

namespace RichbetsResurrected.SharedKernel.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}