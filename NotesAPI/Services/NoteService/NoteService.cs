using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Data;
using NotesAPI.Dtos.Note;
using NotesAPI.Models;

namespace NotesAPI.Services.NoteService
{
    public class NoteService : INoteService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public NoteService(
            DataContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetNoteDto>> GetAllNotes()
        {
			var notes = await _context.Notes
                .Select(n => _mapper.Map<GetNoteDto>(n))
                .ToListAsync();
			return notes;
        }

        public async Task<GetNoteDto> SubmitNote(CreateNoteDto newNote)
        {
            var note = _mapper.Map<Note>(newNote);
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            var noteToReturn = _mapper.Map<GetNoteDto>(note);
            return noteToReturn;
        }
    }
}
