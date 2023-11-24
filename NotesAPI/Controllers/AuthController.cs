using Microsoft.AspNetCore.Mvc;
using NotesAPI.Models;
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
			var user = await _authService.RegisterUser(newUser);
			return Ok(user);
		}

		[HttpPost("login")]
		public async Task<ActionResult<string>> LoginUser(LoginUserDto requestedUser)
		{
			string jwt = await _authService.LoginUser(requestedUser);
			return Ok(jwt);
		}
	}
}
