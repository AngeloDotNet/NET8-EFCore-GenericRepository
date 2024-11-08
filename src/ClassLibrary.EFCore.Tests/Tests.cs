using ClassLibrary.EFCore.Tests.DatabaseContext;
using ClassLibrary.EFCore.Tests.Entities;
using Xunit;

namespace ClassLibrary.EFCore.Tests;

public class Tests : InMemoryDbContext
{
    [Fact]
    public async Task GetAllEntities()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entities = await repository.GetAllAsync();

        Assert.NotNull(entities);
    }

    [Fact]
    public async Task GetAllEntitiesWithPredicateAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entities = await repository.GetAllEntitiesAsync(predicate: x => x.Id is >= 3 and <= 8);

        Assert.Equal(6, entities.Count);
        Assert.NotNull(entities);
    }

    [Fact]
    public async Task GetEntityByIdAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entity = await repository.GetByIdAsync(2);

        Assert.NotNull(entity);
        Assert.Equal(2, Assert.IsType<Person>(entity).Id);
        Assert.Equal("Nome2", Assert.IsType<Person>(entity).Nome);
        Assert.Equal("Cognome2", Assert.IsType<Person>(entity).Cognome);
    }

    [Fact]
    public async Task GetEntityByIdNotFoundAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entity = await repository.GetByIdAsync(30);

        Assert.Null(entity);
    }

    [Fact]
    public async Task CreateEntityAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entity = new Person
        {
            Nome = "Nome11",
            Cognome = "Cognome11"
        };

        await repository.CreateAsync(entity);

        Assert.NotNull(entity);
        Assert.Equal(11, entity.Id);
        Assert.Equal("Nome11", entity.Nome);
        Assert.Equal("Cognome11", entity.Cognome);
    }

    [Fact]
    public async Task UpdateEntityAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entity = await repository.GetByIdAsync(2);

        entity.Nome = "Nome2-bis";
        entity.Cognome = "Cognome2-bis";

        await repository.UpdateAsync(entity);

        Assert.NotNull(entity);
        Assert.Equal(2, entity.Id);
        Assert.Equal("Nome2-bis", entity.Nome);
        Assert.Equal("Cognome2-bis", entity.Cognome);
    }

    [Fact]
    public async Task DeleteEntityAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entity = await repository.GetByIdAsync(4);
        await repository.DeleteAsync(entity!);

        var entities = await repository.GetAllAsync();

        Assert.NotNull(entities);
        Assert.Equal(9, entities.Count);
    }

    [Fact]
    public async Task DeleteByIdEntityAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        await repository.DeleteByIdAsync(4);

        var entities = await repository.GetAllAsync();

        Assert.NotNull(entities);
        Assert.Equal(9, entities.Count);
    }

    [Fact]
    public async Task GetPaginatedEntitiesAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entities = await repository.GetPaginatedAsync(null!, x => x.Id <= 10, x => x.Id, true, 2, 5);

        Assert.NotNull(entities);
        Assert.Equal(5, entities.Count);
        Assert.Contains(entities, x => x.Id == 8);
    }

    [Fact]
    public async Task GetPaginatedEntitiesWithoutIncludeAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entities = await repository.GetPaginatedAsync(null!, x => x.Id <= 10, x => x.Id, true, 2, 5);

        Assert.NotNull(entities);
        Assert.Equal(5, entities.Count);
        Assert.Contains(entities, x => x.Id == 8);
    }

    [Fact]
    public async Task GetPaginatedEntitiesWithoutWhereAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entities = await repository.GetPaginatedAsync(null!, null!, x => x.Id, true, 1, 5);

        Assert.NotNull(entities);
        Assert.Equal(5, entities.Count);
        Assert.Contains(entities, x => x.Id == 3);
    }

    [Fact]
    public async Task GetPaginatedEntitiesDescendingOrderTypeAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Person, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entities = await repository.GetPaginatedAsync(null!, null!, x => x.Id, false, 1, 5);

        Assert.NotNull(entities);
        Assert.Equal(5, entities.Count);
        Assert.Equal(10, Assert.IsType<Person>(entities.First()).Id);
        Assert.Contains(entities, x => x.Id == 8);
    }
}