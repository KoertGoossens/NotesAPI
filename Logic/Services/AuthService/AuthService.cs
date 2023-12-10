using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Logic.Dtos.User;
using Data.Models;
using Data.Repositories.UserRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Linq;

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
            user.Role = "User";
            user.DateCreated = DateTime.Now;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            user.PasswordHash = passwordHash;

            await _userRepository.CreateUser(user);

            var userToReturn = _mapper.Map<GetUserDto>(user);
            return userToReturn;
        }

        public async Task<LoginResult> LoginUser(LoginUserDto requestedUser)
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

            var loginResult = await CreateTokens(user);
            return loginResult;
        }

        public async Task<LoginResult> RefreshToken(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshToken(refreshToken);

			if (user.TokenExpires < DateTime.Now)
			{
				throw new Exception("Refresh token expired.");
			}

            var loginResult = await CreateTokens(user);
            return loginResult;
        }

        public async Task RemoveRefreshToken(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshToken(refreshToken);

            user.RefreshToken = string.Empty;
			user.TokenCreated = DateTime.MinValue;
			user.TokenExpires = DateTime.MinValue;

            await _userRepository.StoreRefreshToken();
        }

        private async Task<LoginResult> CreateTokens(User user)
        {
            var newRefreshToken = GenerateRefreshToken();
            await StoreRefreshToken(user, newRefreshToken);

            string jwt = CreateJwt(user);

            var loginResult = new LoginResult {
                accessToken = jwt,
                refreshToken = newRefreshToken
            };

            return loginResult;
        }

        private string CreateJwt(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            if (jwt == null)
            {
                throw new Exception("Could not create JWT.");
            }

            return jwt;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(30)
            };

            return refreshToken;
        }

        private async Task StoreRefreshToken(User user, RefreshToken refreshToken)
        {
            user.RefreshToken = refreshToken.Token;
			user.TokenCreated = refreshToken.Created;
			user.TokenExpires = refreshToken.Expires;

            await _userRepository.StoreRefreshToken();
        }
    }
}
