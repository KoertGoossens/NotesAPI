using FluentValidation;
using Logic.Dtos.User;

namespace NotesAPI.Validation.User
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(user => user.Username).MinimumLength(3);
            RuleFor(user => user.Password).MinimumLength(8);
        }
    }
}
