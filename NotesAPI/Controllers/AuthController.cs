using Microsoft.AspNetCore.Mvc;
using Logic.Dtos.User;
using Logic.Services.AuthService;
using NotesAPI.Validation;
using FluentValidation;
using NotesAPI.ExceptionHandling;

namespace NotesAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private IAuthService _authService;
		private IValidator<LoginUserDto> _loginUserDtoValidator;
		private IValidator<CreateUserDto> _createUserDtoValidator;

		public AuthController(
			IAuthService authService,
			IValidator<LoginUserDto> loginUserDtoValidator,
			IValidator<CreateUserDto> createUserDtoValidator)
		{
			_authService = authService;
			_loginUserDtoValidator = loginUserDtoValidator;
			_createUserDtoValidator = createUserDtoValidator;
		}

		[HttpPost("register")]
		public async Task<ActionResult<ServiceResponse<GetUserDto>>> RegisterUser(CreateUserDto newUser)
		{
			var result = await _createUserDtoValidator.ValidateAsync(newUser);

			if (!result.IsValid)
			{
				throw new ApiValidationException(result.Errors);
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
			var result = await _loginUserDtoValidator.ValidateAsync(requestedUser);

			if (!result.IsValid)
			{
				throw new ApiValidationException(result.Errors);
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
