using FluentValidation;
using Logic.Dtos.Note;

namespace NotesAPI.Validation.Note
{
	public class CreateNoteDtoValidator : AbstractValidator<CreateNoteDto>
    {
        public CreateNoteDtoValidator()
        {
            RuleFor(note => note.Title).NotEmpty();
            RuleFor(note => note.Content).NotEmpty();
        }
    }
}
