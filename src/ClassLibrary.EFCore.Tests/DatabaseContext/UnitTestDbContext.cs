using ClassLibrary.EFCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.EFCore.Tests.DatabaseContext;

public class UnitTestDbContext(DbContextOptions<UnitTestDbContext> options) : DbContext(options)
{
    public DbSet<Person> People { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasData(new Person { Id = 1, Cognome = "Cognome1", Nome = "Nome1" },
                new Person { Id = 2, Cognome = "Cognome2", Nome = "Nome2" },
                new Person { Id = 3, Cognome = "Cognome3", Nome = "Nome3" },
                new Person { Id = 4, Cognome = "Cognome4", Nome = "Nome4" },
                new Person { Id = 5, Cognome = "Cognome5", Nome = "Nome5" },
                new Person { Id = 6, Cognome = "Cognome6", Nome = "Nome6" },
                new Person { Id = 7, Cognome = "Cognome7", Nome = "Nome7" },
                new Person { Id = 8, Cognome = "Cognome8", Nome = "Nome8" },
                new Person { Id = 9, Cognome = "Cognome9", Nome = "Nome9" },
                new Person { Id = 10, Cognome = "Cognome10", Nome = "Nome10" }
            );
    }
}