﻿using Data.Models;

namespace Data.Repositories.UserRepository
{
	public interface IUserRepository
	{
		Task<User> GetUserByUsername(string username);
		Task<bool> CheckUsernameAvailable(string username);
		Task CreateUser(User user);
	}
}