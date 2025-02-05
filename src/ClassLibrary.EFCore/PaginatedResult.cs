namespace ClassLibrary.EFCore.Interfaces;

/// <summary>
/// Represents a paginated result set.
/// </summary>
/// <typeparam name="T">The type of the items in the result set.</typeparam>
public class PaginatedResult<T>
{
    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Gets or sets the size of each page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

    /// <summary>
    /// Gets or sets the items in the current page.
    /// </summary>
    public List<T> Items { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedResult{T}"/> class.
    /// </summary>
    public PaginatedResult()
    {
        Items = [];
    }
}
