using AutoMapper;
using Logic.Dtos.Note;
using Logic.Dtos.User;
using Data.Models;

namespace Logic
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
