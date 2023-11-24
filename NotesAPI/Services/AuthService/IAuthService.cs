using NotesAPI.Dtos.User;
using NotesAPI.Models;

namespace NotesAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<GetUserDto> RegisterUser(CreateUserDto newUser);
        Task<string> LoginUser(LoginUserDto requestedUser);
    };
}
