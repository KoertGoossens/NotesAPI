﻿using AutoMapper;
using Logic.Dtos.Note;
using Data.Models;
using Data.Repositories.NoteRepository;
using Logic.Services.UserService;

namespace Logic.Services.NoteService
{
	public class NoteService : INoteService
    {
        private readonly IMapper _mapper;
        private IUserService _userService;
        private INoteRepository _noteRepository;

        public NoteService(
            IMapper mapper,
            IUserService userService,
            INoteRepository noteRepository)
        {
            _mapper = mapper;
            _userService = userService;
            _noteRepository = noteRepository;
        }

        public async Task<GetNoteDto> GetNoteById(int id)
		{
			var note = await _noteRepository.GetNoteByIdWithCreator(id);
			var noteToReturn = _mapper.Map<GetNoteDto>(note);
			return noteToReturn;
		}

        public async Task<List<GetNoteForListDto>> GetAllNotes()
        {
            var notes = await _noteRepository.GetAllNotes();
            var notesForList = notes.Select(n => _mapper.Map<GetNoteForListDto>(n)).ToList();
			return notesForList;
        }

        public async Task<GetNoteDto> CreateNote(CreateNoteDto newNote)
        {
            var note = _mapper.Map<Note>(newNote);

            var currentUser = await _userService.GetCurrentUser();

            note.CreatorId = currentUser.Id;
            note.TimeCreated = DateTime.Now;

            await _noteRepository.CreateNote(note);

            var noteToReturn = _mapper.Map<GetNoteDto>(note);
            return noteToReturn;
        }

        public async Task<GetNoteDto> UpdateNote(UpdateNoteDto editedNote)
        {
            var note = await _noteRepository.GetNoteById(editedNote.Id);

            await CheckUserIsCreator(note.CreatorId, editedNote.Id);

			note.Title = editedNote.Title;
			note.Content = editedNote.Content;
            
			await _noteRepository.UpdateNote();

			var noteToReturn = _mapper.Map<GetNoteDto>(note);
            return noteToReturn;
        }

        public async Task DeleteNote(int id)
        {
            var note = await _noteRepository.GetNoteById(id);

            await CheckUserIsCreator(note.CreatorId, id);

            await _noteRepository.DeleteNote(note);
        }

        public async Task CheckUserIsCreator(int creatorId, int noteId)
        {
            var currentUser = await _userService.GetCurrentUser();

            if (creatorId != currentUser.Id)
            {
                throw new Exception($"User is not authorized to delete note with Id '{noteId}'");
            }
        }
    }
}
