namespace ClassLibrary.EFCore;

public class Repository<TEntity, TKey>(DbContext dbContext) : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    /// <summary>
    /// Gets the database context.
    /// </summary>
    public DbContext DbContext { get; } = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <summary>
    /// Retrieves all entities asynchronously with optional filtering, ordering, and including related entities.
    /// </summary>
    /// <param name="includes">A function to include related entities.</param>
    /// <param name="filter">A filter expression to apply.</param>
    /// <param name="orderBy">An expression to order the results.</param>
    /// <param name="ascending">A boolean indicating whether the order should be ascending.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IQueryable of TEntity.</returns>
    public async Task<IQueryable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>,
        IIncludableQueryable<TEntity, object>> includes = null!,
        Expression<Func<TEntity, bool>> filter = null!,
        Expression<Func<TEntity, object>> orderBy = null!,
        bool ascending = true)
    {
        var query = DbContext.Set<TEntity>().AsNoTracking();

        if (includes != null)
        {
            query = includes(query);
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        return await Task.FromResult(query);
    }

    /// <summary>
    /// Retrieves an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
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
    /// Creates a new entity asynchronously.
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
    /// Updates an existing entity asynchronously.
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
    /// Deletes an entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);

        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteByIdAsync(TKey id)
    {
        var entity = new TEntity { Id = id };

        DbContext.Set<TEntity>().Remove(entity);

        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves a paginated result of entities asynchronously.
    /// </summary>
    /// <param name="query">The query to paginate.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a PaginatedResult of TEntity.</returns>
    public async Task<PaginatedResult<TEntity>> GetPaginatedAsync(IQueryable<TEntity> query, int pageNumber, int pageSize)
    {
        var result = new PaginatedResult<TEntity>
        {
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalItems = await query.CountAsync(),
            Items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
        };

        return result;
    }
}