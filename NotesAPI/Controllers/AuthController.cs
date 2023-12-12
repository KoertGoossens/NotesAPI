using Microsoft.AspNetCore.Mvc;
using Logic.Dtos.User;
using Logic.Services.AuthService;
using FluentValidation;
using NotesAPI.ExceptionHandling;
using Data.Models;

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

			var loginResult = await _authService.LoginUser(requestedUser);

			SetRefreshTokenCookie(loginResult.refreshToken);

			var response = new ServiceResponse<string>();
			response.Data = loginResult.accessToken;
			return Ok(response);
		}

		[HttpPost("refreshtoken")]
		public async Task<ActionResult<ServiceResponse<string>>> RefreshToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];
			
			if (refreshToken == null)
			{
				throw new RefreshTokenNotFoundException("Refresh token not found.");
			}

			var loginResult = await _authService.RefreshToken(refreshToken);

			SetRefreshTokenCookie(loginResult.refreshToken);

			var response = new ServiceResponse<string>();
			response.Data = loginResult.accessToken;
			return Ok(response);
		}

		[HttpPost("logout")]
		public async Task<ActionResult<ServiceResponse<object>>> LogoutUser()
		{
			var refreshToken = Request.Cookies["refreshToken"];
			
			if (refreshToken != null)
			{
				var cookieOptions = new CookieOptions
				{
					HttpOnly = true,
					Secure = true,
					SameSite = SameSiteMode.None
				};

				Response.Cookies.Delete("refreshToken", cookieOptions);

				await _authService.RemoveRefreshToken(refreshToken);
			}

			var response = new ServiceResponse<object>();
			return Ok(response);
		}

		private void SetRefreshTokenCookie(RefreshToken refreshToken)
		{
			var cookieOptions = new CookieOptions
			{
                HttpOnly = true,
                Expires = refreshToken.Expires,
				Secure = true,
				SameSite = SameSiteMode.None
			};

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
		}
	}
}
