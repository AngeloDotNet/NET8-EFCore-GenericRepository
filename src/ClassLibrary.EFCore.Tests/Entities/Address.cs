namespace ClassLibrary.EFCore.Tests.Entities;

public class Address
{
    public int Id { get; set; }
    public string Citta { get; set; } = null!;
    public string Cap { get; set; } = null!;
    public string Provincia { get; set; } = null!;
}