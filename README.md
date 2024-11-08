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
    Task<IEnumerable<YourEntity>> GetAllAsync();
    Task<YourEntity> GetByIdAsync(int id);
    Task CreateAsync(YourEntity entity);
    Task UpdateAsync(YourEntity entity);
    Task DeleteAsync(YourEntity entity);

    //Alternative method for extracting all records
    Task<IEnumerable<YourEntity>> GetAllEntitiesAsync(Func<YourEntity, bool> predicate)

    //Alternative method for deleting
    Task DeleteByIdAsync(int id);

    //Optional method
    Task<List<YourEntity>> GetPaginatedAsync(Func<IQueryable<YourEntity>,
        IIncludableQueryable<YourEntity, object>> includes,
        Expression<Func<YourEntity, bool>> conditionWhere,
        Expression<Func<YourEntity, dynamic>> orderBy,
        bool ascending, int pageIndex, int pageSize);
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

    public async Task<IEnumerable<YourEntity>> GetAllAsync()
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

    //Alternative method for extracting all records
    public async Task<IEnumerable<YourEntity>> GetAllEntitiesAsync(Func<YourEntity, bool> predicate)
    {
        return await _repository.GetAllEntitiesAsync(predicate);
    }

    //Alternative method for deleting
    public async Task DeleteByIdAsync(int id)
    {
        await _repository.DeleteByIdAsync(id);
    }

    //Optional method for pagination
    //If ascending is passed to true, the list is sorted in ascending order.
    //If ascending is passed to false, the list is sorted in descending order.
    public Task<List<YourEntity>> GetPaginatedAsync(Func<IQueryable<YourEntity>,
        IIncludableQueryable<YourEntity, object>> includes, Expression<Func<YourEntity, bool>> conditionWhere,
        Expression<Func<YourEntity, dynamic>> orderBy, bool ascending, int pageIndex, int pageSize)
    {
        return await _repository.GetPaginatedAsync(includes, conditionWhere, orderBy, ascending, pageIndex, pageSize);
    }
}
```

## Contributing

Contributions and/or suggestions are always welcome.