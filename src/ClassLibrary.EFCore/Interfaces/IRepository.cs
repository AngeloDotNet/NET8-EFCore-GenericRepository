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
    /// Asynchronously gets all entities that satisfy the given predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities that satisfy the condition.</returns>
    Task<List<TEntity>> GetAllEntitiesAsync(Func<TEntity, bool> predicate);

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
    /// Asynchronously gets a paginated list of entities that satisfy the given conditions.
    /// </summary>
    /// <param name="includes">A function to include related entities.</param>
    /// <param name="conditionWhere">A function to test each element for a condition.</param>
    /// <param name="orderBy">A function to order the elements.</param>
    /// <param name="ascending">A boolean indicating whether the order is ascending.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities that satisfy the conditions.</returns>
    public Task<List<TEntity>> GetPaginatedAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
        Expression<Func<TEntity, bool>> conditionWhere, Expression<Func<TEntity, dynamic>> orderBy, bool ascending, int pageIndex, int pageSize);
}
