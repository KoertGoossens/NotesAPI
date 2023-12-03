﻿namespace Data.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public string Role { get; set; } = "User";
		public DateTime DateCreated { get; set; }

		public List<Note> Notes { get; set; }
	}
}
