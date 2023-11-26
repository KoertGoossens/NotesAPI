namespace NotesAPI
{
	public class ServiceResponse<T>
	{
		public T? Data { get; set; }
		public int StatusCode { get; set; } = 200;
		public string Message { get; set; } = string.Empty;
	}
}
