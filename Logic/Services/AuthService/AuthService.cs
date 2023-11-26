using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Logic.Dtos.User;
using Data.Models;
using Data.Repositories.UserRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Logic.Services.AuthService
{
	public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private IUserRepository _userRepository;

        public AuthService(
            IConfiguration configuration,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<GetUserDto> RegisterUser(CreateUserDto newUser)
		{
            bool usernameAvailable = await _userRepository.CheckUsernameAvailable(newUser.Username);

            if (!usernameAvailable)
            {
                throw new Exception("Username not available.");
            }

            var user = new User();
            user.Username = newUser.Username;
            user.Email = newUser.Email;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            user.PasswordHash = passwordHash;

            await _userRepository.CreateUser(user);

            var userToReturn = _mapper.Map<GetUserDto>(user);
            return userToReturn;
        }

        public async Task<string> LoginUser(LoginUserDto requestedUser)
        {
            var user = await _userRepository.GetUserByUsername(requestedUser.Username);

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
