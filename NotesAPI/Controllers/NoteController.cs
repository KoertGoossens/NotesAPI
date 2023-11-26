using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logic.Dtos.Note;
using Logic.Dtos.User;
using Data.Models;
using Logic.Services.NoteService;

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
		public async Task<ActionResult<ServiceResponse<GetNoteDto>>> GetNoteById(int id)
		{
			var note = await _noteService.GetNoteById(id);

			var response = new ServiceResponse<GetNoteDto>();
			response.Data = note;

			return Ok(response);
		}

		[HttpGet("getall")]
		public async Task<ActionResult<ServiceResponse<List<GetNoteForListDto>>>> GetAllNotes()
		{
			var notes = await _noteService.GetAllNotes();

			var response = new ServiceResponse<List<GetNoteForListDto>>();
			response.Data = notes;

			return Ok(response);
		}

		[HttpPost("submit")]
		public async Task<ActionResult<ServiceResponse<GetNoteDto>>> CreateNote(CreateNoteDto newNote)
		{
			if (newNote.Title.Length == 0)
			{
				throw new Exception("Title cannot be empty.");
			}
			if (newNote.Content.Length == 0)
			{
				throw new Exception("Content cannot be empty.");
			}

			var note = await _noteService.CreateNote(newNote);

			var response = new ServiceResponse<GetNoteDto>();
			response.Data = note;

			return Ok(response);
		}

		[HttpPut("edit")]
		public async Task<ActionResult<ServiceResponse<GetNoteDto>>> UpdateNote(UpdateNoteDto editedNote)
		{
			if (editedNote.Title.Length == 0)
			{
				throw new Exception("Title cannot be empty.");
			}
			if (editedNote.Content.Length == 0)
			{
				throw new Exception("Content cannot be empty.");
			}

			var note = await _noteService.UpdateNote(editedNote);

			var response = new ServiceResponse<GetNoteDto>();
			response.Data = note;

			return Ok(response);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<ServiceResponse<object>>> DeleteNote(int id)
		{
			await _noteService.DeleteNote(id);

			var response = new ServiceResponse<object>();
			return Ok(response);
		}
	}
}
