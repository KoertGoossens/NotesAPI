using AutoMapper;
using Logic.Dtos.User;
using Data.Repositories.UserRepository;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Logic.Services.UserService
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
            if (_httpContextAccessor.HttpContext == null)
            {
                throw new Exception("HttpContext not found.");
            }

            var username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            if (username == null)
            {
                throw new Exception("Username of current user not found.");
            }

            var user = await _userRepository.GetUserByUsername(username);

            if (user == null)
			{
				throw new Exception($"User with username '{username}' not found.");
			}

            var userToReturn = _mapper.Map<GetUserDto>(user);
            return userToReturn;
        }

        public async Task<List<GetUserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            var usersToReturn = users.Select(u => _mapper.Map<GetUserDto>(u)).ToList();
			return usersToReturn;
        }
    }
}
