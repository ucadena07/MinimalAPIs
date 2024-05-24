using FluentValidation;
using MinimalAPIsMovies.Dtos;

namespace MinimalAPIsMovies.Validation
{
    public class CreateActorValidator : AbstractValidator<CreateActorDTO>
    {
        public CreateActorValidator()
        {
            RuleFor(p => p.Name).NotEmpty()
                                .MaximumLength(150).WithMessage(ValidationUtils.MaxLengthMessage)
                                .NotEmpty().WithMessage(ValidationUtils.NonEmptyMessage);
            var minDate = new DateTime(1900, 1, 1);
            RuleFor(p => p.DoB).GreaterThanOrEqualTo(minDate).WithMessage("The field {PropertyName} should be greater than " + minDate.ToString("MM/dd/yyyy"));
        }
    }
}
