using FluentValidation;
using MinimalAPIsMovies.Dtos;

namespace MinimalAPIsMovies.Validation
{
    public class UserCreationDTOValidator : AbstractValidator<UserCredentialsDTO>
    {
        public UserCreationDTOValidator()
        {
            RuleFor(it => it.Email).NotEmpty().WithMessage(ValidationUtils.NonEmptyMessage)
                .MaximumLength(255).WithMessage(ValidationUtils.MaxLengthMessage)
                .EmailAddress().WithMessage(ValidationUtils.EmailMessage);
            RuleFor(it => it.Password).NotEmpty().WithMessage(ValidationUtils.NonEmptyMessage);

        }
    }
}
