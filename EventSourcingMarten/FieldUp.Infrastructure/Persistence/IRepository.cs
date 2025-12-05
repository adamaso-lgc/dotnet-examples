namespace FieldUp.Infrastructure.Persistence;

public interface IRepository<TEntity, in TId>
    where TEntity : class
{
    Task<TEntity?> GetAsync(TId id, CancellationToken ct = default);
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default);
    Task SaveAsync(TEntity entity, CancellationToken ct = default);
    Task DeleteAsync(TId id, CancellationToken ct = default);
}
