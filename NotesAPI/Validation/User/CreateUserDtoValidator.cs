using FluentValidation;
using Logic.Dtos.User;

namespace NotesAPI.Validation.User
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(user => user.Username).MinimumLength(3);
            RuleFor(user => user.Email).EmailAddress();
            RuleFor(user => user.Password).MinimumLength(8);
        }
    }
}
