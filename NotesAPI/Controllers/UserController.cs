using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesAPI.Models;
using NotesAPI.Services.UserService;
using System.Security.Claims;

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
		public async Task<ActionResult<User>> GetCurrentUser()
		{
			var user = await _userService.GetCurrentUser();
            return Ok(user);
		}
	}
}
