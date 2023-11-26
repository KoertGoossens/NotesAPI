using Data.Models;

namespace Data.Repositories.NoteRepository
{
	public interface INoteRepository
	{
		Task<Note> GetNoteById(int id);
		Task<Note> GetNoteByIdWithCreator(int id);
		Task<List<Note>> GetAllNotes();
		Task CreateNote(Note note);
		Task UpdateNote();
		Task DeleteNote(Note note);
	}
}
