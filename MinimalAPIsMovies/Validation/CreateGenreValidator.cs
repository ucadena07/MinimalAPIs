using FluentValidation;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.Validation
{
    public class CreateGenreValidator : AbstractValidator<Genre>
    {
        public CreateGenreValidator(IGenresRepository _repo)
        {
                RuleFor(p => p.Name)
                .MaximumLength(150).WithMessage(ValidationUtils.MaxLengthMessage)
                .NotEmpty().WithMessage(ValidationUtils.NonEmptyMessage)
                .Must(ValidationUtils.FirstLetterIsUppercase).WithMessage(ValidationUtils.FirstLetterIsUppercaseMessage)
                .MustAsync(async (name,_) =>
                {
                    var exists = await _repo.Exists(0, name);
                    return !exists;
                }).WithMessage(g => $"A genre with the name {g.Name} already exists");    
        }



    }
}
