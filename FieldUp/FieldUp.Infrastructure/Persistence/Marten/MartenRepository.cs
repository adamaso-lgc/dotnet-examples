using Marten;

namespace FieldUp.Infrastructure.Persistence.Marten;

public class MartenRepository<TEntity, TId>(IDocumentStore store) : IRepository<TEntity, TId>
    where TEntity : class
{
    public async Task<TEntity?> GetAsync(TId id, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        await using var session = store.QuerySession();
        return await session.LoadAsync<TEntity>(id, ct);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        await using var session = store.QuerySession();
        return await session.Query<TEntity>().ToListAsync(ct);
    }

    public async Task SaveAsync(TEntity entity, CancellationToken ct = default)
    {
        await using var session = store.LightweightSession();

        session.Store(entity);
        await session.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(TId id, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        await using var session = store.LightweightSession();

        session.Delete<TEntity>(id);
        await session.SaveChangesAsync(ct);
    }
}
