using Microsoft.AspNetCore.Mvc;
using NotesAPI.Dtos.User;
using NotesAPI.Services.AuthService;

namespace NotesAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<ActionResult<GetUserDto>> RegisterUser(CreateUserDto newUser)
		{
			if (newUser.Username == null)
			{
				throw new Exception("Username cannot be empty.");
			}
			if (newUser.Email == null)
			{
				throw new Exception("Email cannot be empty.");
			}
			if (newUser.Password == null)
			{
				throw new Exception("Password cannot be empty.");
			}

			var user = await _authService.RegisterUser(newUser);
			return Ok(user);
		}

		[HttpPost("login")]
		public async Task<ActionResult<string>> LoginUser(LoginUserDto requestedUser)
		{
			if (requestedUser.Username == null)
			{
				throw new Exception("Username cannot be empty.");
			}
			if (requestedUser.Password == null)
			{
				throw new Exception("Password cannot be empty.");
			}

			string jwt = await _authService.LoginUser(requestedUser);
			return Ok(jwt);
		}
	}
}
