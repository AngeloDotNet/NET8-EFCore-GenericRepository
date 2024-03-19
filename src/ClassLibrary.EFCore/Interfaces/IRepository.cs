namespace ClassLibrary.EFCore.Interfaces;

public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    /// <summary>
    /// Gets the list of entities
    /// </summary>
    Task<List<TEntity>> GetAllAsync();

    /// <summary>
    /// Gets the single entity based on the given ID
    /// </summary>
    /// <param name="id"></param>
    Task<TEntity?> GetByIdAsync(TKey id);

    /// <summary>
    /// Creating a new entity
    /// </summary>
    /// <param name="entity"></param>
    Task CreateAsync(TEntity entity);

    /// <summary>
    /// Updating a pre-existing entity
    /// </summary>
    /// <param name="entity"></param>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Deletion of an entity
    /// </summary>
    /// <param name="entity"></param>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Deletion of an entity based on the given ID
    /// </summary>
    /// <param name="id"></param>
    Task DeleteByIdAsync(TKey id);

    /// <summary>
    /// Gets the list of entities and allows you to apply filters and pagination
    /// </summary>
    /// <param name="includes"></param>
    /// <param name="conditionWhere"></param>
    /// <param name="orderBy"></param>
    /// <param name="orderType"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    Task<List<TEntity>> GetPaginatedAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
        Expression<Func<TEntity, bool>> conditionWhere, Expression<Func<TEntity, dynamic>> orderBy, string orderType,
        int pageIndex, int pageSize);
}