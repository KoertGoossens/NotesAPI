using Logic.Dtos.User;

namespace Logic.Dtos.Note
{
	public class GetNoteDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public DateTime TimeCreated { get; set; }

		public int CreatorId { get; set; }
		public GetUserDto Creator { get; set; }
	}
}
