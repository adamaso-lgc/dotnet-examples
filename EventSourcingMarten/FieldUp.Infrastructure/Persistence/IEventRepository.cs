using FieldUp.Domain.Core;

namespace FieldUp.Infrastructure.Persistence;

public interface IEventRepository<TAggregateRoot, in TId>
    where TAggregateRoot : class, IAggregateRoot<TId>, new()
{
    Task SaveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);
    Task<TAggregateRoot?> GetAsync(TId id, CancellationToken cancellationToken = default);
}