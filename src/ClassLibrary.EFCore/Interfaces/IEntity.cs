namespace ClassLibrary.EFCore.Interfaces;

/// <summary>
/// Defines a generic entity interface with a key of type TKey.
/// </summary>
/// <typeparam name="TKey">The type of the key for the entity.</typeparam>
public interface IEntity<TKey>
{
    /// <summary>
    /// Gets or sets the identifier for the entity.
    /// </summary>
    TKey Id { get; set; }
}