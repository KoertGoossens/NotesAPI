using AutoMapper;
using NotesAPI.Dtos.Note;
using NotesAPI.Dtos.User;
using NotesAPI.Models;

namespace NotesAPI
{
    public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<User, GetUserDto>();

			CreateMap<Note, GetNoteDto>();
			CreateMap<Note, GetNoteForListDto>();
			CreateMap<CreateNoteDto, Note>();
		}
	}
}
