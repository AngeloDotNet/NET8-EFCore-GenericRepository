using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ClassLibrary.EFCore.Interfaces;

public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    Task<List<TEntity>> GetAllAsync();

    Task<TEntity?> GetByIdAsync(TKey id);

    Task CreateAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task<List<TEntity>> GetPaginatedAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
        Expression<Func<TEntity, bool>> conditionWhere, Expression<Func<TEntity, dynamic>> orderBy, string orderType,
        int pageIndex, int pageSize);
}