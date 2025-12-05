using FieldUp.Domain.Core;
using Marten;

namespace FieldUp.Infrastructure.Persistence.Marten;

public class MartenEventRepository<TAggregateRoot, TId>(IDocumentStore store) : IEventRepository<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot<TId>, new()
{
    private readonly IDocumentStore _store = store ?? throw new ArgumentNullException(nameof(store));

    public async Task<TAggregateRoot?> GetAsync(TId id, CancellationToken ct = default)
    {
        await using var session = await _store.LightweightSerializableSessionAsync(token: ct);

        var aggregate = await session.Events.AggregateStreamAsync<TAggregateRoot>(id?.ToString()!, token: ct);

        return aggregate;
    }

    public async Task SaveAsync(TAggregateRoot aggregate, CancellationToken ct = default)
    {
        await using var session = await _store.LightweightSerializableSessionAsync(ct);

        var events = aggregate.GetUncommittedEvents().ToArray();
        if (events.Length == 0) return;

        session.Events.Append(aggregate.Id?.ToString()!, aggregate.Version, events);

        aggregate.ClearUncommittedEvents();

        await session.SaveChangesAsync(ct);
    }
}
