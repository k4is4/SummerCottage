using System.ComponentModel.DataAnnotations;
using System;
using System.Text.Json;
using Cottage.API.Exceptions;

namespace Cottage.API.Middleware
{
	internal sealed class ExceptionHandlingMiddleware : IMiddleware
	{
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);

				await HandleExceptionAsync(context, e);
			}
		}

		private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
		{
			var statusCode = GetStatusCode(exception);

			var response = new
			{
				title = GetTitle(exception),
				status = GetStatusCode(exception),
				detail = exception.Message,
			};

			httpContext.Response.ContentType = "application/json";

			httpContext.Response.StatusCode = statusCode;

			await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
		}

		private static int GetStatusCode(Exception exception) =>
			exception switch
			{
				ConflictException => StatusCodes.Status409Conflict,
				NotFoundException => StatusCodes.Status404NotFound,
				ValidationsException => StatusCodes.Status400BadRequest,
				_ => StatusCodes.Status500InternalServerError
			};

		private static string GetTitle(Exception exception) =>
		exception switch
		{
				ApplicationsException applicationException => applicationException.Title,
				_ => "Server Error"
			};
	}
}
