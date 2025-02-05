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
    public async Task<PaginatedResult<TEntity>> GetPaginatedAsync(IQueryable<TEntity> query, int pageNumber, int pageSize)
    {
        //For optional lambdas read the comments of the GetAllAsync method
        var query = await repository.GetAllAsync();
        var result = await repository.GetPaginatedAsync(query, 2, 5);

        return result;
    }
}
```

## Contributing

Contributions and/or suggestions are always welcome.