using Microsoft.AspNetCore.Mvc;
using Logic.Dtos.User;
using Data.Models;
using Logic.Services.AuthService;

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
		public async Task<ActionResult<ServiceResponse<GetUserDto>>> RegisterUser(CreateUserDto newUser)
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

			var response = new ServiceResponse<GetUserDto>();
			response.Data = user;

			return Ok(response);
		}

		[HttpPost("login")]
		public async Task<ActionResult<ServiceResponse<string>>> LoginUser(LoginUserDto requestedUser)
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

			var response = new ServiceResponse<string>();
			response.Data = jwt;

			return Ok(response);
		}
	}
}
