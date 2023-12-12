using Logic.ExceptionHandling;
using NotesAPI.ExceptionHandling;
using Serilog;
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
			catch (ApiConflictException ex)
			{
				await WriteHttpResponse(
					context,
					(int)HttpStatusCode.Conflict,
					ex.ErrorMessage);
			}
			catch (ApiInvalidUserException ex)
			{
				await WriteHttpResponse(
					context,
					(int)HttpStatusCode.NotFound,
					ex.ErrorMessage);
			}
			catch (RefreshTokenNotFoundException ex)
			{
				await WriteHttpResponse(
					context,
					(int)HttpStatusCode.NotFound,
					ex.ErrorMessage);
			}
			catch (ApiValidationException ex)
			{
				foreach(var error in ex.Errors)
				{
					Log.Error("Validation error: " + error.ErrorMessage);
				}

				await WriteHttpResponse(
					context,
					(int)HttpStatusCode.BadRequest,
					"One or more validation errors occurred.");
			}
			catch (Exception ex)
			{
				Log.Error("Generic error: " + ex.Message);

				await WriteHttpResponse(
					context,
					(int)HttpStatusCode.InternalServerError,
					"An unknown backend error occurred.");
			}
		}

		public async Task WriteHttpResponse(HttpContext context, int statusCode, string errorMessage)
		{
			context.Response.StatusCode = statusCode;
			context.Response.ContentType = "application/json";

			var response = new ServiceResponse<object>();
			response.StatusCode = statusCode;
			response.Message = errorMessage;

			string json = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(json);
		}
	}
}
