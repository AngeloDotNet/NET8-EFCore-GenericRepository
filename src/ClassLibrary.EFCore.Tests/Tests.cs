using ClassLibrary.EFCore.Tests.DatabaseContext;
using ClassLibrary.EFCore.Tests.Entities;
using Xunit;

namespace ClassLibrary.EFCore.Tests;

public class Tests : InMemoryDbContext
{
    [Fact]
    public void GetAllEntities()
    {
        using var dbContext = GetDbContext();
        Repository<Person, int> repository = new(dbContext);

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        var entities = repository.GetAllAsync();

        Assert.NotNull(entities);
    }

    [Fact]
    public void GetEntityById()
    {
        using var dbContext = GetDbContext();
        Repository<Person, int> repository = new(dbContext);

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        var entity = repository.GetByIdAsync(2);

        Assert.NotNull(entity);
        Assert.Equal(2, Assert.IsType<Person>(entity?.Result).Id);
        Assert.Equal("Nome2", Assert.IsType<Person>(entity?.Result).Nome);
        Assert.Equal("Cognome2", Assert.IsType<Person>(entity?.Result).Cognome);
    }

    [Fact]
    public void GetEntityByIdNotFound()
    {
        using var dbContext = GetDbContext();
        Repository<Person, int> repository = new(dbContext);

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        var entity = repository.GetByIdAsync(30).Result;

        Assert.Null(entity);
    }

    [Fact]
    public void CreateEntity()
    {
        using var dbContext = GetDbContext();
        Repository<Person, int> repository = new(dbContext);

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        var entity = new Person
        {
            Nome = "Nome11",
            Cognome = "Cognome11"
        };

        repository?.CreateAsync(entity);

        Assert.NotNull(entity);
        Assert.Equal(11, entity.Id);
        Assert.Equal("Nome11", entity.Nome);
        Assert.Equal("Cognome11", entity.Cognome);
    }

    [Fact]
    public void UpdateEntity()
    {
        using var dbContext = GetDbContext();
        Repository<Person, int> repository = new(dbContext);

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        var entity = repository.GetByIdAsync(2).Result;

        entity!.Nome = "Nome2-bis";
        entity!.Cognome = "Cognome2-bis";

        repository?.UpdateAsync(entity);

        Assert.NotNull(entity);
        Assert.Equal(2, entity.Id);
        Assert.Equal("Nome2-bis", entity.Nome);
        Assert.Equal("Cognome2-bis", entity.Cognome);
    }

    [Fact]
    public void DeleteEntity()
    {
        using var dbContext = GetDbContext();
        Repository<Person, int> repository = new(dbContext);

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        var entity = repository.GetByIdAsync(4).Result;
        repository?.DeleteAsync(entity!);

        var entities = repository?.GetAllAsync();

        Assert.NotNull(entities);
        Assert.Equal(9, entities?.Result.Count);
    }

    [Fact]
    public void GetPaginatedEntities()
    {
        using var dbContext = GetDbContext();
        Repository<Person, int> repository = new(dbContext);

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        var entities = repository.GetPaginatedAsync(null, x => x.Id <= 10, x => x.Id, "ASC", 2, 5);

        Assert.NotNull(entities);
        Assert.Equal(5, entities?.Result.Count);
        Assert.Contains(entities!.Result, x => x.Id == 8);
    }

    [Fact]
    public void GetPaginatedEntitiesWithoutInclude()
    {
        using var dbContext = GetDbContext();
        Repository<Person, int> repository = new(dbContext);

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        var entities = repository.GetPaginatedAsync(null, x => x.Id <= 10, x => x.Id, "ASC", 2, 5);

        Assert.NotNull(entities);
        Assert.Equal(5, entities?.Result.Count);
        Assert.Contains(entities!.Result, x => x.Id == 8);
    }

    [Fact]
    public void GetPaginatedEntitiesWithoutWhere()
    {
        using var dbContext = GetDbContext();
        Repository<Person, int> repository = new(dbContext);

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        var entities = repository.GetPaginatedAsync(null, null, x => x.Id, "ASC", 1, 5);

        Assert.NotNull(entities);
        Assert.Equal(5, entities?.Result.Count);
        Assert.Contains(entities!.Result, x => x.Id == 3);
    }

    [Fact]
    public void GetPaginatedEntitiesWithoutOrderType()
    {
        using var dbContext = GetDbContext();
        Repository<Person, int> repository = new(dbContext);

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        var entities = repository.GetPaginatedAsync(null, null, x => x.Id, null, 2, 5);

        Assert.NotNull(entities);
        Assert.Equal(5, entities?.Result.Count);
        Assert.Equal(6, Assert.IsType<Person>(entities?.Result.First()).Id);
        Assert.Contains(entities!.Result, x => x.Id == 8);
    }

    [Fact]
    public void GetPaginatedEntitiesDescendingOrderType()
    {
        using var dbContext = GetDbContext();
        Repository<Person, int> repository = new(dbContext);

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        var entities = repository.GetPaginatedAsync(null, null, x => x.Id, "DESC", 1, 5);

        Assert.NotNull(entities);
        Assert.Equal(5, entities?.Result.Count);
        Assert.Equal(10, Assert.IsType<Person>(entities?.Result.First()).Id);
        Assert.Contains(entities!.Result, x => x.Id == 8);
    }
}