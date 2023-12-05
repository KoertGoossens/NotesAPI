using FluentValidation.Results;

namespace NotesAPI.ExceptionHandling
{
	public class ApiValidationException : Exception
	{
		public List<ValidationFailure> Errors;

		public ApiValidationException(List<ValidationFailure> errors)
        {
			this.Errors = errors;
        }
	}
}
