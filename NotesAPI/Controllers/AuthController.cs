using Microsoft.AspNetCore.Mvc;
using Logic.Dtos.User;
using Logic.Services.AuthService;
using Data.Models;
using Logic.Dtos.Note;
using System.ComponentModel.DataAnnotations;
using Serilog;

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
			if (newUser.Username.Length < 3)
			{
				throw new Exception("Username must contain at least 3 characters.");
			}
			if (newUser.Email == null)
			{
				throw new Exception("Email cannot be empty.");
			}
			if (newUser.Password.Length < 8)
			{
				throw new Exception("Password must contain at least 8 characters.");
			}

			var user = await _authService.RegisterUser(newUser);

			if (user == null)
            {
                throw new Exception("Failed to register user.");
            }

			var response = new ServiceResponse<GetUserDto>();
			response.Data = user;
			return Ok(response);
		}

		[HttpPost("login")]
		public async Task<ActionResult<ServiceResponse<string>>> LoginUser(LoginUserDto requestedUser)
		{
			if (requestedUser.Username.Length < 3)
			{
				throw new Exception("Username must contain at least 3 characters.");
			}
			if (requestedUser.Password.Length < 8)
			{
				throw new Exception("Password must contain at least 8 characters.");
			}

			string jwt = await _authService.LoginUser(requestedUser);

			if (jwt == null)
            {
                throw new Exception("Failed to login user.");
            }

			var response = new ServiceResponse<string>();
			response.Data = jwt;
			return Ok(response);
		}
	}
}
