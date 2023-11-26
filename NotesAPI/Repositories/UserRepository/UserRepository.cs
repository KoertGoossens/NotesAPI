using Microsoft.EntityFrameworkCore;
using NotesAPI.Data;
using NotesAPI.Models;

namespace NotesAPI.Repositories.UserRepository
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
			var user = await _context.Users.
				FirstOrDefaultAsync(u => u.Username == username);

			return user;
		}

		public async Task<bool> CheckUsernameAvailable(string username)
		{
			bool isAvailable = await _context.Users.
				AnyAsync(u => u.Username == username);

			return !isAvailable;
		}

		public async Task CreateUser(User user)
		{
			await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
		}
	}
}
