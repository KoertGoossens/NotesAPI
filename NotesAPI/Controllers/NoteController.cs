using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesAPI.Dtos;
using NotesAPI.Dtos.Note;
using NotesAPI.Models;
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

		[HttpGet("getall")]
		public async Task<ActionResult<List<GetNoteDto>>> GetAllNotes()
		{
			var notes = await _noteService.GetAllNotes();
			return Ok(notes);
		}

		[HttpPost("submit")]
		public async Task<ActionResult<GetNoteDto>> SubmitNote(CreateNoteDto newNote)
		{
			var note = await _noteService.SubmitNote(newNote);
			return Ok(note);
		}
	}
}
