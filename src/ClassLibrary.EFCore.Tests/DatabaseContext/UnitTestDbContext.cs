using Bogus;
using ClassLibrary.EFCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Persone = ClassLibrary.EFCore.Tests.Entities.Person;

namespace ClassLibrary.EFCore.Tests.DatabaseContext;

public class UnitTestDbContext(DbContextOptions<UnitTestDbContext> options) : DbContext(options)
{
    public DbSet<Persone> Persone { get; set; }
    public DbSet<Address> Indirizzi { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var fakerAddresses = new Faker<Address>("it")
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.Citta, f => f.Address.City())
            .RuleFor(x => x.Cap, f => f.Address.ZipCode())
            .RuleFor(x => x.Provincia, f => f.Address.State());

        var indirizzi = fakerAddresses.Generate(10);
        modelBuilder.Entity<Address>().HasData(indirizzi);

        var fakerPeople = new Faker<Persone>("it")
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.Cognome, f => f.Person.LastName)
            .RuleFor(x => x.Nome, f => f.Person.FirstName)
            .RuleFor(d => d.IndirizzoId, f => f.PickRandom(indirizzi).Id);

        var persone = fakerPeople.Generate(10);
        modelBuilder.Entity<Persone>().HasData(persone);
    }
}