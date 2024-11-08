using ClassLibrary.EFCore.Extensions;

namespace ClassLibrary.EFCore;

/// <summary>
/// Repository class for Entity Framework Core operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
public class Repository<TEntity, TKey>(DbContext dbContext) : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    /// <summary>
    /// Gets the DbContext.
    /// </summary>
    public DbContext DbContext { get; } = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <summary>
    /// Asynchronously gets all entities.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities.</returns>
    public async Task<List<TEntity>> GetAllAsync()
        => await DbContext.Set<TEntity>().AsNoTracking().ToListAsync();

    /// <summary>
    /// Asynchronously gets all entities that satisfy the given predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities that satisfy the condition.</returns>
    public async Task<List<TEntity>> GetAllEntitiesAsync(Func<TEntity, bool> predicate)
        => await Task.FromResult(DbContext.Set<TEntity>().AsNoTracking().Where(predicate).ToList());

    /// <summary>
    /// Asynchronously gets an entity by its id.
    /// </summary>
    /// <param name="id">The id of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        var entity = await DbContext.Set<TEntity>().FindAsync(id);

        if (entity == null)
        {
            return null;
        }

        DbContext.Entry(entity).State = EntityState.Detached;

        return entity;
    }

    /// <summary>
    /// Asynchronously creates a new entity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);

        await DbContext.SaveChangesAsync();

        DbContext.Entry(entity).State = EntityState.Detached;
    }

    /// <summary>
    /// Asynchronously updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);

        await DbContext.SaveChangesAsync();

        DbContext.Entry(entity).State = EntityState.Detached;
    }

    /// <summary>
    /// Asynchronously deletes an existing entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);

        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously deletes an entity by its id.
    /// </summary>
    /// <param name="id">The id of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteByIdAsync(TKey id)
    {
        var entity = new TEntity { Id = id };

        DbContext.Set<TEntity>().Remove(entity);

        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously gets a paginated list of entities.
    /// </summary>
    /// <param name="includes">A function to include related entities.</param>
    /// <param name="conditionWhere">A condition to filter the entities.</param>
    /// <param name="orderBy">A function to order the entities.</param>
    /// <param name="ascending">A boolean indicating whether the order is ascending.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of paginated entities.</returns>
    public Task<List<TEntity>> GetPaginatedAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
        Expression<Func<TEntity, bool>> conditionWhere, Expression<Func<TEntity, dynamic>> orderBy, bool ascending, int pageIndex, int pageSize)
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();

        if (includes != null)
        {
            query = includes(query);
        }

        if (conditionWhere != null)
        {
            query = query.Where(conditionWhere);
        }

        if (orderBy != null)
        {
            query = query.OrderedByAscending(orderBy, ascending);
        }

        return query.Page(pageIndex, pageSize).AsNoTracking().ToListAsync();
    }
}
