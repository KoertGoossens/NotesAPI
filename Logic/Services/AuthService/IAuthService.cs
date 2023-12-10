using Logic.Dtos.User;

namespace Logic.Services.AuthService
{
    public interface IAuthService
    {
        Task<GetUserDto> RegisterUser(CreateUserDto newUser);
        Task<LoginResult> LoginUser(LoginUserDto requestedUser);
        Task<LoginResult> RefreshToken(string refreshToken);
        Task RemoveRefreshToken(string refreshToken);
    };
}
