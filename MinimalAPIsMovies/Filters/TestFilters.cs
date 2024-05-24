
namespace MinimalAPIsMovies.Filters
{
    public class TestFilters : IEndpointFilter
    {
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var r = await next(context);
            return r;   
        }
    }
}
