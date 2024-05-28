
using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.Filters
{
    public class TestFilters : IEndpointFilter
    {
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var param1 = context.Arguments.OfType<int>().FirstOrDefault();   
            var param2 = context.Arguments.OfType<IGenresRepository>().FirstOrDefault();   



            var r = await next(context);
            return r;   
        }
    }
}
