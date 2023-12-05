using System.Net;

namespace NotesAPI
{
	public class ServiceResponse<T>
	{
		public T? Data { get; set; }
		public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
		public string Message { get; set; } = string.Empty;
	}
}
