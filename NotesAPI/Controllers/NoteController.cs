﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logic.Dtos.Note;
using Logic.Services.NoteService;
using NotesAPI.ExceptionHandling;
using FluentValidation;

namespace NotesAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class NoteController : ControllerBase
	{
		private INoteService _noteService;
		private IValidator<CreateNoteDto> _createNoteDtoValidator;
		private IValidator<UpdateNoteDto> _updateNoteDtoValidator;

		public NoteController(
			INoteService noteService,
			IValidator<CreateNoteDto> createNoteDtoValidator,
			IValidator<UpdateNoteDto> updateNoteDtoValidator)
		{
			_noteService = noteService;
			_createNoteDtoValidator = createNoteDtoValidator;
			_updateNoteDtoValidator = updateNoteDtoValidator;
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
			var result = await _createNoteDtoValidator.ValidateAsync(newNote);

			if (!result.IsValid)
			{
				throw new ApiValidationException(result.Errors);
			}
			
			var note = await _noteService.CreateNote(newNote);

			var response = new ServiceResponse<GetNoteDto>();
			response.Data = note;
			return Ok(response);
		}

		[HttpPut("edit")]
		public async Task<ActionResult<ServiceResponse<GetNoteDto>>> UpdateNote(UpdateNoteDto editedNote)
		{
			var result = await _updateNoteDtoValidator.ValidateAsync(editedNote);

			if (!result.IsValid)
			{
				throw new ApiValidationException(result.Errors);
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
