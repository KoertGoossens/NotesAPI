using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotesAPI.Data;
using NotesAPI.Dtos.User;
using NotesAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotesAPI.Services.AuthService
{
	public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AuthService(
            IConfiguration configuration,
            DataContext context,
            IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetUserDto> RegisterUser(CreateUserDto newUser)
        {
            var user = new User();

            user.Username = newUser.Username;
            user.Email = newUser.Email;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            user.PasswordHash = passwordHash;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userToReturn = _mapper.Map<GetUserDto>(user);
            return userToReturn;
        }

        public async Task<string> LoginUser(LoginUserDto requestedUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == requestedUser.Username);

            if (user == null)
            {
                throw new Exception("Wrong username or password.");
            }

            if (!BCrypt.Net.BCrypt.Verify(requestedUser.Password, user.PasswordHash))
            {
                throw new Exception("Wrong username or password.");
            }

            string jwt = CreateJwt(user);
            return jwt;
        }

        private string CreateJwt(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
