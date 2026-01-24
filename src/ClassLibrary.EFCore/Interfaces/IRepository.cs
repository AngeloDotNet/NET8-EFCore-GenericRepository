namespace ClassLibrary.EFCore.Interfaces;

public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    /// <summary>
    /// Retrieves all entities asynchronously with optional filtering, ordering, and including related entities.
    /// </summary>
    /// <param name="includes">A function to include related entities.</param>
    /// <param name="filter">A filter expression to apply.</param>
    /// <param name="orderBy">An expression to order the results.</param>
    /// <param name="ascending">A boolean indicating whether the order should be ascending.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IQueryable of TEntity.</returns>
    Task<IQueryable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>,
        IIncludableQueryable<TEntity, object>> includes = null!, Expression<Func<TEntity, bool>> filter = null!, Expression<Func<TEntity, object>> orderBy = null!,
        bool ascending = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated result of entities asynchronously.
    /// </summary>
    /// <param name="query">The query to paginate.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a PaginatedResult of TEntity.</returns>
    [Obsolete(message: "Use GetAllPagingAsync instead. This method will be removed in a future version.", error: false)]
    Task<PaginatedResult<TEntity>> GetPaginatedAsync(IQueryable<TEntity> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated result of entities asynchronously with optional filtering, ordering, and including related entities.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="includes">A function to include related entities in the query.</param>
    /// <param name="filter">A filter expression to apply to the entities.</param>
    /// <param name="orderBy">An expression to order the results.</param>
    /// <param name="ascending">A boolean indicating whether the order should be ascending.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedResult{TEntity}"/>
    /// with the paginated entities.
    /// </returns>
    Task<PaginatedResult<TEntity>> GetAllPagingAsync(int pageNumber, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null!,
        Expression<Func<TEntity, bool>> filter = null!, Expression<Func<TEntity, object>> orderBy = null!, bool ascending = true, CancellationToken cancellationToken = default);
}
