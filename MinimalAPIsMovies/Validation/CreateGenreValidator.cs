using FluentValidation;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Validation
{
    public class CreateGenreValidator : AbstractValidator<Genre>
    {
        public CreateGenreValidator()
        {
                RuleFor(p => p.Name).NotEmpty();    
        }
    }
}
