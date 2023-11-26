using AutoMapper;
using NotesAPI.Dtos.User;
using NotesAPI.Repositories.UserRepository;
using System.Security.Claims;

namespace NotesAPI.Services.UserService
{
	public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private IUserRepository _userRepository;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<GetUserDto> GetCurrentUser()
        {
            var username = string.Empty;

            if (_httpContextAccessor.HttpContext != null)
            {
                username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            var user = await _userRepository.GetUserByUsername(username);

            if (user == null)
            {
                throw new Exception("Current user not found.");
            }

            var userToReturn = _mapper.Map<GetUserDto>(user);
            return userToReturn;
        }
    }
}
