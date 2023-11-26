using Logic.Dtos.User;

namespace Logic.Services.UserService
{
    public interface IUserService
    {
        Task<GetUserDto> GetCurrentUser();
    }
}
