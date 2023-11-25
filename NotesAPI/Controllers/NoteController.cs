using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesAPI.Dtos.Note;
using NotesAPI.Services.NoteService;

namespace NotesAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class NoteController : ControllerBase
	{
		private INoteService _noteService;

		public NoteController(INoteService noteService)
		{
			_noteService = noteService;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<GetNoteDto>> GetNoteById(int id)
		{
			var note = await _noteService.GetNoteById(id);
			return Ok(note);
		}

		[HttpGet("getall")]
		public async Task<ActionResult<List<GetNoteForListDto>>> GetAllNotes()
		{
			var notes = await _noteService.GetAllNotes();
			return Ok(notes);
		}

		[HttpPost("submit")]
		public async Task<ActionResult<GetNoteDto>> CreateNote(CreateNoteDto newNote)
		{
			var note = await _noteService.CreateNote(newNote);
			return Ok(note);
		}

		[HttpPut("edit")]
		public async Task<ActionResult<GetNoteDto>> UpdateNote(UpdateNoteDto editedNote)
		{
			var note = await _noteService.UpdateNote(editedNote);
			return Ok(note);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteNote(int id)
		{
			await _noteService.DeleteNote(id);
			return Ok();
		}
	}
}
