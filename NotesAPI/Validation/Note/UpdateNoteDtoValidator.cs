using FluentValidation;
using Logic.Dtos.Note;

namespace NotesAPI.Validation.Note
{
	public class UpdateNoteDtoValidator : AbstractValidator<UpdateNoteDto>
    {
        public UpdateNoteDtoValidator()
        {
            RuleFor(note => note.Title).NotEmpty();
            RuleFor(note => note.Content).NotEmpty();
        }
    }
}
