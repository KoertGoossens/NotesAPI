using NotesAPI.Dtos.User;

namespace NotesAPI.Services.UserService
{
    public interface IUserService
    {
        Task<GetUserDto> GetCurrentUser();
    }
}
