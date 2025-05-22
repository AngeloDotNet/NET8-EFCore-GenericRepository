using Bogus;
using ClassLibrary.EFCore.Tests.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Persone = ClassLibrary.EFCore.Tests.Entities.Person;

namespace ClassLibrary.EFCore.Tests;

public class Tests : InMemoryDbContext
{
    [Fact]
    public async Task GetAllEntitiesAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entities = await repository.GetAllAsync();

        Assert.NotNull(entities);
    }

    [Fact]
    public async Task GetAllEntitiesWithFilterAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entities = await repository.GetAllAsync(includes: null!, filter: x => x.Id >= 3 & x.Id <= 8);

        Assert.Equal(6, entities.Count());
        Assert.NotNull(entities);
    }

    [Fact]
    public async Task GetEntityByIdAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entity = await repository.GetByIdAsync(2);

        Assert.NotNull(entity);
        Assert.Equal(2, Assert.IsType<Persone>(entity).Id);
        Assert.Equal(entity.Nome, Assert.IsType<Persone>(entity).Nome);
        Assert.Equal(entity.Cognome, Assert.IsType<Persone>(entity).Cognome);
    }

    [Fact]
    public async Task GetEntityByIdNotFoundAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entity = await repository.GetByIdAsync(30);

        Assert.Null(entity);
    }

    [Fact]
    public async Task CreateEntityAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var personFaker = new Faker<Persone>("it")
            .RuleFor(p => p.Id, f => f.IndexFaker + f.Random.Number(11, 100))
            .RuleFor(p => p.Cognome, f => f.Person.LastName)
            .RuleFor(p => p.Nome, f => f.Person.FirstName)
            .RuleFor(p => p.IndirizzoId, f => f.Random.Int(1, 10));

        var entity = personFaker.Generate();

        await repository.CreateAsync(personFaker);

        Assert.NotNull(entity);
        Assert.True(entity.Id > 0);
        Assert.True(entity.Id <= 100);
        Assert.False(string.IsNullOrEmpty(entity.Nome));
        Assert.False(string.IsNullOrEmpty(entity.Cognome));
    }

    [Fact]
    public async Task UpdateEntityAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entity = await repository.GetByIdAsync(2);

        if (entity == null)
        {
            return;
        }

        var personFaker = new Faker<Persone>("it")
            .RuleFor(p => p.Cognome, f => f.Person.LastName)
            .RuleFor(p => p.Nome, f => f.Person.FirstName);

        var newEntity = personFaker.Generate();

        entity.Nome = newEntity.Nome;
        entity.Cognome = newEntity.Cognome;

        await repository.UpdateAsync(entity);

        Assert.NotNull(entity);
        Assert.Equal(2, Assert.IsType<Persone>(entity).Id);
        Assert.Equal(newEntity.Nome, Assert.IsType<Persone>(entity).Nome);
        Assert.Equal(newEntity.Cognome, Assert.IsType<Persone>(entity).Cognome);
    }

    [Fact]
    public async Task DeleteEntityAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var entity = await repository.GetByIdAsync(4);
        await repository.DeleteAsync(entity!);

        var entities = await repository.GetAllAsync();

        Assert.NotNull(entities);
        Assert.Equal(9, entities.Count());
    }

    [Fact]
    public async Task DeleteByIdEntityAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        await repository.DeleteByIdAsync(4);

        var entities = await repository.GetAllAsync();

        Assert.NotNull(entities);
        Assert.Equal(9, entities.Count());
    }

    [Fact]
    public async Task GetPaginatedEntitiesAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var query = await repository.GetAllAsync(includes: q => q.Include(p => p.Indirizzo), filter: w => w.Id <= 10);
        var entities = await repository.GetPaginatedAsync(query, 2, 5);
        var itemCount = entities.Items.Count;

        Assert.NotNull(entities);
        Assert.Equal(5, itemCount);
        Assert.Contains(query, x => x.Id == 8);
    }

    [Fact]
    public async Task GetPaginatedEntitiesWithoutIncludeAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var query = await repository.GetAllAsync(includes: q => q.Include(p => p.Indirizzo), filter: w => w.Id <= 10);

        var entities = await repository.GetPaginatedAsync(query, 2, 5);
        var itemCount = entities.Items.Count;

        Assert.NotNull(entities);
        Assert.Equal(5, itemCount);
        Assert.Contains(query, x => x.Id == 8);
    }

    [Fact]
    public async Task GetPaginatedEntitiesWithoutWhereAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var query = await repository.GetAllAsync(includes: q => q.Include(p => p.Indirizzo), filter: null!, orderBy: x => x.Id);

        var entities = await repository.GetPaginatedAsync(query, 1, 5);
        var itemCount = entities.Items.Count;

        Assert.NotNull(entities);
        Assert.Equal(5, itemCount);
        Assert.Contains(query, x => x.Id == 3);
    }

    [Fact]
    public async Task GetPaginatedEntitiesDescendingOrderTypeAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var query = await repository.GetAllAsync(includes: q => q.Include(p => p.Indirizzo), filter: null!, orderBy: x => x.Id, ascending: false);

        var entities = await repository.GetPaginatedAsync(query, 1, 5);
        var itemCount = entities.Items.Count;

        Assert.NotNull(entities);
        Assert.Equal(5, itemCount);
        Assert.Equal(10, Assert.IsType<Persone>(entities.Items.First()).Id);
        Assert.Contains(query, x => x.Id == 8);
    }

    [Fact]
    public async Task GetPagingEntitiesAsync()
    {
        using var dbContext = GetDbContext();
        var repository = new Repository<Persone, int>(dbContext);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var result = await repository.GetAllPagingAsync(pageNumber: 2, pageSize: 5, includes: q => q.Include(p => p.Indirizzo), filter: w => w.Id <= 10);

        Assert.NotNull(result);
        Assert.Equal(10, result.TotalItems);
        Assert.Equal(5, result.Items.Count);
        Assert.Contains(result.Items, x => x.Id == 8);
    }
}