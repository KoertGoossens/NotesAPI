using Microsoft.EntityFrameworkCore;
using NotesAPI.Models;

namespace NotesAPI.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) {}

		public DbSet<User> Users { get; set; }
		public DbSet<Note> Notes { get; set; }
	}
}
