namespace Logic.ExceptionHandling
{
	public class ApiConflictException : Exception
	{
		public string ErrorMessage;

		public ApiConflictException(string errorMessage)
        {
			this.ErrorMessage = errorMessage;
        }
	}
}
