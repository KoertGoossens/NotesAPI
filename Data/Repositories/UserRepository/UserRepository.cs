using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Data.Repositories.UserRepository
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _context;

		public UserRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<User> GetUserByUsername(string username)
		{
			var user = await _context.Users
				.FirstOrDefaultAsync(u => u.Username == username);

			if (user == null)
			{
				throw new Exception($"User with username '{username}' not found.");
			}

			return user;
		}

		public async Task<List<User>> GetAllUsers()
		{
			var users = await _context.Users.ToListAsync();

            if (users == null)
            {
                throw new Exception("Failed to load list of users.");
            }

			return users;
		}

		public async Task<bool> CheckUsernameAvailable(string username)
		{
			bool isAvailable = await _context.Users
				.AnyAsync(u => u.Username == username);

			return !isAvailable;
		}

		public async Task CreateUser(User user)
		{
			try
			{
				await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to create user.");
			}
		}

		public async Task<User> GetUserByRefreshToken(string refreshToken)
		{
			var user = await _context.Users
				.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

			if (user == null)
			{
				throw new Exception("Invalid refresh token.");
			}

			return user;
		}

		public async Task StoreRefreshToken()
		{
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to store refresh token.");
			}
		}
	}
}
