using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Data.Repositories.NoteRepository
{
	public class NoteRepository : INoteRepository
	{
		private readonly DataContext _context;

		public NoteRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<Note> GetNoteById(int id)
		{
			var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.Id == id);

			return note;
		}

		public async Task<Note> GetNoteByIdWithCreator(int id)
		{
			var note = await _context.Notes
                .Include(n => n.Creator)
                .FirstOrDefaultAsync(n => n.Id == id);

			return note;
		}

		public async Task<List<Note>> GetAllNotes()
		{
			var notes = await _context.Notes
                .Include(n => n.Creator)
                .ToListAsync();

			return notes;
		}

		public async Task CreateNote(Note note)
		{
			await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
		}

		public async Task UpdateNote()
		{
			await _context.SaveChangesAsync();
		}

		public async Task DeleteNote(Note note)
		{
		    _context.Notes.Remove(note);
			await _context.SaveChangesAsync();
		}
	}
}
