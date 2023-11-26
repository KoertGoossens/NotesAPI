namespace Data.Models
{
	public class Note
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;

		public int CreatorId { get; set; }
		public User Creator { get; set; }
	}
}
