
using FluentValidation;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Filters
{
    public class GenresValidationFilter : IEndpointFilter
    {
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<Genre>>();
            if(validator == null)
            {
                return await next(context);
            }
            var obj = context.Arguments.OfType<Genre>().FirstOrDefault();   
            if(obj == null)
            {
                return Results.Problem("The object couldn not be found.");
            }
            var validationResult = validator.Validate(obj);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
            return await next(context);
        }
    }
}
