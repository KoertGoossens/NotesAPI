using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logic.Dtos.User;
using Logic.Services.UserService;

namespace NotesAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetCurrentUser()
		{
			var user = await _userService.GetCurrentUser();

			var response = new ServiceResponse<GetUserDto>();
			response.Data = user;
            return Ok(response);
		}

		[HttpGet("getall")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> GetAllUsers()
		{
			var users = await _userService.GetAllUsers();

			var response = new ServiceResponse<List<GetUserDto>>();
			response.Data = users;
            return Ok(response);
		}
	}
}
