using NotesAPI.Dtos.Note;
using NotesAPI.Models;

namespace NotesAPI.Services.NoteService
{
    public interface INoteService
    {
        Task<List<GetNoteDto>> GetAllNotes();
        Task<GetNoteDto> SubmitNote(CreateNoteDto newNote);
    }
}
