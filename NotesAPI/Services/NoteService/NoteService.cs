using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Data;
using NotesAPI.Dtos.Note;
using NotesAPI.Models;
using NotesAPI.Services.UserService;

namespace NotesAPI.Services.NoteService
{
    public class NoteService : INoteService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private IUserService _userService;

        public NoteService(
            DataContext context,
            IMapper mapper,
            IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<GetNoteDto> GetNoteById(int id)
		{
			var note = await _context.Notes
                .Include(n => n.Creator)
                .FirstOrDefaultAsync(n => n.Id == id);

			var noteToReturn = _mapper.Map<GetNoteDto>(note);
			return noteToReturn;
		}

        public async Task<List<GetNoteForListDto>> GetAllNotes()
        {
			var notes = await _context.Notes
                .Include(n => n.Creator)
                .Select(n => _mapper.Map<GetNoteForListDto>(n))
                .ToListAsync();
			return notes;
        }

        public async Task<GetNoteDto> CreateNote(CreateNoteDto newNote)
        {
            var note = _mapper.Map<Note>(newNote);

            var currentUser = await _userService.GetCurrentUser();
            note.CreatorId = currentUser.Id;

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            var noteToReturn = _mapper.Map<GetNoteDto>(note);
            return noteToReturn;
        }

        public async Task<GetNoteDto> UpdateNote(UpdateNoteDto editedNote)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == editedNote.Id);

			if (note == null)
			{
				throw new Exception($"Note with Id '{editedNote.Id}' not found.");
			}

            CheckUserIsCreator(note.CreatorId, editedNote.Id);

			note.Title = editedNote.Title;
			note.Content = editedNote.Content;
            
			await _context.SaveChangesAsync();

			var noteToReturn = _mapper.Map<GetNoteDto>(note);
            return noteToReturn;
        }

        public async Task DeleteNote(int id)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
			{
				throw new Exception($"Note with Id '{id}' not found.");
			}

            CheckUserIsCreator(note.CreatorId, id);

            _context.Notes.Remove(note);
			await _context.SaveChangesAsync();
        }

        public async void CheckUserIsCreator(int creatorId, int noteId)
        {
            var currentUser = await _userService.GetCurrentUser();

            if (creatorId != currentUser.Id)
            {
                throw new Exception($"User is not authorized to delete note with Id '{noteId}'");
            }
        }
    }
}
