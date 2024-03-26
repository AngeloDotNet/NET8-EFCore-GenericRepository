namespace ClassLibrary.EFCore.Interfaces;

/// <summary>
/// Repository interface for entity operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    /// <summary>
    /// Asynchronously gets all entities.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities.</returns>
    Task<List<TEntity>> GetAllAsync();

    /// <summary>
    /// Asynchronously gets the entity by its id.
    /// </summary>
    /// <param name="id">The id of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found, null otherwise.</returns>
    Task<TEntity?> GetByIdAsync(TKey id);

    /// <summary>
    /// Asynchronously creates a new entity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateAsync(TEntity entity);

    /// <summary>
    /// Asynchronously updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Asynchronously deletes an existing entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Asynchronously deletes an entity by its id.
    /// </summary>
    /// <param name="id">The id of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteByIdAsync(TKey id);

    /// <summary>
    /// Asynchronously gets a paginated list of entities.
    /// </summary>
    /// <param name="includes">A function to include related entities.</param>
    /// <param name="conditionWhere">A condition to filter the entities.</param>
    /// <param name="orderBy">A function to order the entities.</param>
    /// <param name="orderType">The type of the order.</param>
    /// <param name="pageIndex">The index of the page.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities.</returns>
    Task<List<TEntity>> GetPaginatedAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
        Expression<Func<TEntity, bool>> conditionWhere, Expression<Func<TEntity, dynamic>> orderBy, string orderType,
        int pageIndex, int pageSize);
}
