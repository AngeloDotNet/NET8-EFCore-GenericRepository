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
    /// <param name="conditionWhere">A function to filter the entities.</param>
    /// <param name="orderBy">A function to sort the entities.</param>
    /// <param name="orderType">The type of the order ("ASC" for ascending, "DESC" for descending).</param>
    /// <param name="pageIndex">The index of the page.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities.</returns>
    public Task<List<TEntity>> GetPaginatedAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
    Expression<Func<TEntity, bool>> conditionWhere, Expression<Func<TEntity, dynamic>> orderBy, string orderType, int pageIndex, int pageSize)
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
            query = orderType switch
            {
                "ASC" => query.OrderBy(orderBy),
                "DESC" => query.OrderByDescending(orderBy),
                _ => query.OrderBy(orderBy),
            };
        }

        if (pageIndex != 0 && pageSize != 0)
        {
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        return query.AsNoTracking().ToListAsync();
    }
}
