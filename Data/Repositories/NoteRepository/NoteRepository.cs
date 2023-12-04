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

            if (note == null)
			{
				throw new Exception($"Note with Id '{id}' not found.");
			}

			return note;
		}

		public async Task<Note> GetNoteByIdWithCreator(int id)
		{
			var note = await _context.Notes
                .Include(n => n.Creator)
                .FirstOrDefaultAsync(n => n.Id == id);

			if (note == null)
			{
				throw new Exception($"Note with Id '{id}' not found.");
			}

			return note;
		}

		public async Task<List<Note>> GetAllNotes()
		{
			var notes = await _context.Notes
                .Include(n => n.Creator)
                .ToListAsync();

			if (notes == null)
            {
                throw new Exception("Failed to load list of notes.");
            }

			return notes;
		}

		public async Task CreateNote(Note note)
		{
			try
			{
				await _context.Notes.AddAsync(note);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to create note.");
			}
		}

		public async Task UpdateNote()
		{
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to update note.");
			}
		}

		public async Task DeleteNote(Note note)
		{
			try
			{
				_context.Notes.Remove(note);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to delete note.");
			}
		}
	}
}
