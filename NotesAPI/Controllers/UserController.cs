using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesAPI.Dtos.User;
using NotesAPI.Services.UserService;

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

		[HttpGet, Authorize]
		public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetCurrentUser()
		{
			var user = await _userService.GetCurrentUser();

			var response = new ServiceResponse<GetUserDto>();
			response.Data = user;

            return Ok(response);
		}
	}
}
