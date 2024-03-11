namespace ClassLibrary.EFCore.Interfaces;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}