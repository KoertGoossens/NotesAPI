using Logic.Dtos.User;

namespace Logic.Services.AuthService
{
    public interface IAuthService
    {
        Task<GetUserDto> RegisterUser(CreateUserDto newUser);
        Task<string> LoginUser(LoginUserDto requestedUser);
    };
}
