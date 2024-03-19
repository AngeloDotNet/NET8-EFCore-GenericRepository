namespace ClassLibrary.EFCore;

public class Repository<TEntity, TKey>(DbContext dbContext) : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    public DbContext DbContext { get; } = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <summary>
    /// Gets the list of entities
    /// </summary>
    public async Task<List<TEntity>> GetAllAsync()
        => await DbContext.Set<TEntity>().AsNoTracking().ToListAsync();

    /// <summary>
    /// Gets the single entity based on the given ID
    /// </summary>
    /// <param name="id"></param>
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
    /// Creating a new entity
    /// </summary>
    /// <param name="entity"></param>
    public async Task CreateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);

        await DbContext.SaveChangesAsync();

        DbContext.Entry(entity).State = EntityState.Detached;
    }

    /// <summary>
    /// Updating a pre-existing entity
    /// </summary>
    /// <param name="entity"></param>
    public async Task UpdateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);

        await DbContext.SaveChangesAsync();

        DbContext.Entry(entity).State = EntityState.Detached;
    }

    /// <summary>
    /// Deletion of an entity
    /// </summary>
    /// <param name="entity"></param>
    public async Task DeleteAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);

        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Deletion of an entity based on the given ID
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteByIdAsync(TKey id)
    {
        var entity = new TEntity { Id = id };

        DbContext.Set<TEntity>().Remove(entity);

        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Gets the list of entities and allows you to apply filters and pagination
    /// </summary>
    /// <param name="includes"></param>
    /// <param name="conditionWhere"></param>
    /// <param name="orderBy"></param>
    /// <param name="orderType"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
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