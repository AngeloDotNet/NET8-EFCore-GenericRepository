namespace ClassLibrary.EFCore.Extensions;

public static class RepositoryExtensions
{
    /// <summary>
    /// Paginates the queryable source by skipping the specified number of items and taking the specified page size.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The queryable source to paginate.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A queryable source that contains the paginated items.</returns>
    public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
                => source.Skip((page - 1) * pageSize).Take(pageSize);

    /// <summary>
    /// Orders the queryable source by the specified order expression in ascending or descending order.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="sources">The queryable source to order.</param>
    /// <param name="orderBy">The expression to order by.</param>
    /// <param name="ascending">A boolean value indicating whether to order in ascending (true) or descending (false) order. Default is true.</param>
    /// <returns>A queryable source that is ordered according to the specified expression and order direction.</returns>
    public static IQueryable<TSource> OrderedByAscending<TSource>(this IQueryable<TSource> sources,
        Expression<Func<TSource, dynamic>> orderBy, bool ascending = true)
    {
        return ascending switch
        {
            false => sources.OrderByDescending(orderBy),
            _ => sources.OrderBy(orderBy)
        };
    }

    //public static IQueryable<TSource> OrderByAscending<TSource>(this IQueryable<TSource> sources, Expression<Func<TSource, dynamic>> orderBy)
    //    => sources.OrderBy(orderBy);

    //public static IQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> sources, Expression<Func<TSource, dynamic>> orderBy)
    //    => sources.OrderByDescending(orderBy);
}