using ClassLibrary.EFCore.Interfaces;

namespace ClassLibrary.EFCore.Tests.Entities;

public class Person : IEntity<int>
{
    public int Id { get; set; }
    public string Cognome { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
}