using NotesAPI.Dtos.Note;

namespace NotesAPI.Services.NoteService
{
	public interface INoteService
    {
        Task<GetNoteDto> GetNoteById(int id);
        Task<List<GetNoteForListDto>> GetAllNotes();
        Task<GetNoteDto> CreateNote(CreateNoteDto newNote);
        Task<GetNoteDto> UpdateNote(UpdateNoteDto editedNote);
        Task DeleteNote(int id);
    }
}
