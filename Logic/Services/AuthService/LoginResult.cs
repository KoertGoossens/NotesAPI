using Data.Models;

namespace Logic.Services.AuthService
{
	public class LoginResult
	{
		public string accessToken = string.Empty;
		public RefreshToken refreshToken;
	}
}
