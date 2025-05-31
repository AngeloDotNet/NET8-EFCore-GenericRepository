# NET8 EFCore Generic Repository
Collection of a generic implementation of Entity Framework Core for .NET 8 mostly used in my private and/or work projects thus avoiding the duplication of repetitive code.

## Give a star
If you found this Implementation helpful or used it in your Projects, do give it a :star: on Github. Thanks!

## Installation
The library is available on [NuGet](https://www.nuget.org/packages/ClassLibrary.EFCore) or run the following command in the .NET CLI:

```bash
dotnet add package ClassLibrary.EFCore
```

## How to use

### Registering services

```csharp
services.AddDbContext<YourDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
services.AddScoped<DbContext, YourDbContext>();
services.AddScoped<IRepository<YourEntity, YourTypeEntityId>, Repository<YourEntity, YourTypeEntityId>>();
```

Alternatively the generic version of the repository can be registered as follows:

```csharp
services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
```

## Important

In this README the INT type is used as ID type, but it is also possible to use the GUID type, making the appropriate corrections later.

## Example entity

```csharp
public class YourEntity : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

## Example interface

```csharp
public interface IYourEntityService
{
    Task<IQueryable<YourEntity>> GetAllAsync(Func<IQueryable<YourEntity>,
        IIncludableQueryable<YourEntity, object>> includes = null!,
        Expression<Func<YourEntity, bool>> filter = null!,
        Expression<Func<YourEntity, object>> orderBy = null!,
        bool ascending = true);

    Task<YourEntity> GetByIdAsync(int id);
    Task CreateAsync(YourEntity entity);
    Task UpdateAsync(YourEntity entity);
    Task DeleteAsync(YourEntity entity);

    //Alternative method for deleting
    Task DeleteByIdAsync(int id);

    //Optional method
    Task<PaginatedResult<TEntity>> GetPaginatedAsync(IQueryable<TEntity> query, int pageNumber, int pageSize);

    //Optional method for pagination (Prefer this method instead of GetPaginatedAsync)
    Task<PaginatedResult<TEntity>> GetAllPagingAsync(int pageNumber, int pageSize, Func<IQueryable<TEntity>,
        IIncludableQueryable<TEntity, object>> includes = null!, Expression<Func<TEntity, bool>> filter = null!,
        Expression<Func<TEntity, object>> orderBy = null!, bool ascending = true);
}
```

## Example class

```csharp
public class YourEntityService : IYourEntityService
{
    private readonly IRepository<YourEntity, int> _repository;

    public YourEntityService(IRepository<YourEntity, int> repository)
    {
        _repository = repository;
    }

    //This method accepts optional lambdas as input (includes, where, order by),
    //while by default it is sorted in ascending order (set false if you want to sort in descending order)
    public async Task<IQueryable<TEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<YourEntity> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateAsync(YourEntity entity)
    {
        await _repository.CreateAsync(entity);
    }

    public async Task UpdateAsync(YourEntity entity)
    {
        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(YourEntity entity)
    {
        await _repository.DeleteAsync(entity);
    }

    //Alternative method for deleting
    public async Task DeleteByIdAsync(int id)
    {
        await _repository.DeleteByIdAsync(id);
    }

    //Optional method for pagination
    //For optional lambdas read the comments of the GetAllAsync method
    //A simple way to retrieve data is to call the GetAllAsync() method
    //After retrieving the data, it will be possible to invoke the GetPaginatedAsync() method to have a paginated list
    //Example: var query = await repository.GetAllAsync(); var result = await repository.GetPaginatedAsync(query, 1, 10);
    public async Task<PaginatedResult<TEntity>> GetPaginatedAsync(IQueryable<TEntity> query, int pageNumber, int pageSize)
    {
        return await repository.GetPaginatedAsync(query, pageNumber, pageSize);
    }

    //Optional method for pagination (Prefer this method instead of GetPaginatedAsync)
    public async Task<PaginatedResult<TEntity>> GetAllPagingAsync(int pageNumber, int pageSize, Func<IQueryable<TEntity>,
        IIncludableQueryable<TEntity, object>> includes = null!, Expression<Func<TEntity, bool>> filter = null!,
        Expression<Func<TEntity, object>> orderBy = null!, bool ascending = true)
    {
        return await repository.await repository.GetAllPagingAsync(pageNumber: 2, pageSize: 5, includes: q => q.Include(p => p.Indirizzo), filter: w => w.Id <= 10);
    }
}
```

<!--
## Test results

![Test Results](your_image_link_here)
-->

## Contributing

Contributions and/or suggestions are always welcome.