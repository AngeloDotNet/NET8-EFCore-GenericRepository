using ClassLibrary.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Threading;

namespace ClassLibrary.EFCore;

public class Repository<TEntity, TKey>(DbContext dbContext) : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    public DbContext DbContext { get; } = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await DbContext.Set<TEntity>().AsNoTracking().ToListAsync();
    }

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

    public async Task CreateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);

        await DbContext.SaveChangesAsync();

        DbContext.Entry(entity).State = EntityState.Detached;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);

        await DbContext.SaveChangesAsync();

        DbContext.Entry(entity).State = EntityState.Detached;
    }

    public async Task DeleteAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);

        await DbContext.SaveChangesAsync();
    }

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
            switch (orderType)
            {
                case "ASC":
                    query = query.OrderBy(orderBy);
                    break;

                case "DESC":
                    query = query.OrderByDescending(orderBy);
                    break;

                default:
                    query = query.OrderBy(orderBy);
                    break;
            }
        }

        if (pageIndex != 0 && pageSize != 0)
        {
            var skip = (pageIndex - 1) * pageSize;

            query = query.Skip(skip).Take(pageSize);
        }

        return query.AsNoTracking().ToListAsync();
    }
}