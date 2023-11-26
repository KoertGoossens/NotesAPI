using System.Net;
using System.Text.Json;

namespace NotesAPI.Middleware
{
	public class ExceptionHandlingMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				context.Response.ContentType = "application/json";

				var response = new ServiceResponse<object>();
				response.StatusCode = (int)HttpStatusCode.InternalServerError;
				response.Message = ex.Message;

				string json = JsonSerializer.Serialize(response);
				await context.Response.WriteAsync(json);
			}
		}
	}
}
