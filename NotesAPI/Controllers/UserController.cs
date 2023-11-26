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
		public async Task<ActionResult<GetUserDto>> GetCurrentUser()
		{
			var user = await _userService.GetCurrentUser();
            return Ok(user);
		}
	}
}
