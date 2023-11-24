using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Data;
using NotesAPI.Dtos.User;
using System.Security.Claims;

namespace NotesAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(
            DataContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetUserDto> GetCurrentUser()
        {
            var username = string.Empty;

            if (_httpContextAccessor.HttpContext != null)
            {
                username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var userToReturn = _mapper.Map<GetUserDto>(user);
            return userToReturn;
        }
    }
}
