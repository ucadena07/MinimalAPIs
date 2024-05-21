using MinimalAPIsMovies.Dtos;

namespace MinimalAPIsMovies.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> quaryable, PaginationDTO pagination)
    {
        return quaryable.Skip((pagination.Page - 1) * pagination.RecordsPerPage).Take(pagination.RecordsPerPage);
    }
}
