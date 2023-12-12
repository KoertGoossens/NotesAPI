namespace Logic.ExceptionHandling
{
	public class ApiInvalidUserException : Exception
	{
		public string ErrorMessage;

		public ApiInvalidUserException(string errorMessage)
        {
			this.ErrorMessage = errorMessage;
        }
	}
}
