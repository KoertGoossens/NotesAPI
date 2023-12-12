namespace NotesAPI.ExceptionHandling
{
	public class RefreshTokenNotFoundException : Exception
	{
		public string ErrorMessage;

		public RefreshTokenNotFoundException(string errorMessage)
        {
			this.ErrorMessage = errorMessage;
        }
	}
}
