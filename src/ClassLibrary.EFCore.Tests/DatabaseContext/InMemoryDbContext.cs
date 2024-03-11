using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.EFCore.Tests.DatabaseContext;

public abstract class InMemoryDbContext
{
    protected UnitTestDbContext GetDbContext()
    {
        var inMemoryDatabase = new DbContextOptionsBuilder<UnitTestDbContext>()
            .UseInMemoryDatabase("InMemory-Database-Test")
            .Options;

        return new UnitTestDbContext(inMemoryDatabase);
    }
}