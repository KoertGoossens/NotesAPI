using NotesAPI.Dtos.User;

namespace NotesAPI.Dtos.Note
{
	public class GetNoteForListDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;

		public int CreatorId { get; set; }
		public GetUserDto Creator { get; set; }
	}
}
