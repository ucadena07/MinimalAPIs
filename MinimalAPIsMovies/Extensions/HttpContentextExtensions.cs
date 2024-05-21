using Microsoft.EntityFrameworkCore;

namespace MinimalAPIsMovies.Extensions;

public static class HttpContentextExtensions
{
    public async static Task InsertPaginationParametersInReponseHeader<T>(
        this HttpContext httpContext, IQueryable<T> queryable)
    {
        if(httpContext == null) throw new ArgumentNullException(nameof(httpContext));
        double count = await queryable.CountAsync();    
        httpContext.Response.Headers.Append("totalAmountOfRecords", count.ToString());  

    }
}
