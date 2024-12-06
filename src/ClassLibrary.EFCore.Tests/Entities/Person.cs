using ClassLibrary.EFCore.Interfaces;

namespace ClassLibrary.EFCore.Tests.Entities;

public class Person : IEntity<int>
{
    public int Id { get; set; }
    public string Cognome { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public int IndirizzoId { get; set; }
    public Address Indirizzo { get; set; } = default!;
}