using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logic.Dtos.User;
using Logic.Services.UserService;
using Data.Models;
using static Azure.Core.HttpHeader;

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

			if (user == null)
            {
                throw new Exception("Current user not found.");
            }

			var response = new ServiceResponse<GetUserDto>();
			response.Data = user;

            return Ok(response);
		}

		[HttpGet("getall")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> GetAllUsers()
		{
			var users = await _userService.GetAllUsers();

			if (users == null)
            {
                throw new Exception("Failed to load list of users.");
            }

			var response = new ServiceResponse<List<GetUserDto>>();
			response.Data = users;

            return Ok(response);
		}
	}
}
