using Cottage.API.Exceptions;
using Cottage.API.Middleware;
using Cottage.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cottage.API.Tests
{
	[TestClass]
	public class ExceptionHandlingMiddlewareTests
	{
		private Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
		private ExceptionHandlingMiddleware _middleware;

		[TestInitialize]
		public void Initialize()
		{
			_loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
			_middleware = new ExceptionHandlingMiddleware(_loggerMock.Object);
		}

		[TestMethod]
		public async Task MiddlewarePassesRequest_WhenNoExceptionOccurs()
		{
			var context = new DefaultHttpContext();

			async Task next(HttpContext ctx)
			{
				await Task.CompletedTask;
			}

			await _middleware.InvokeAsync(context, next);

			// Check that the response hasn't been altered (e.g. StatusCode remains 200 OK)
			Assert.AreEqual(StatusCodes.Status200OK, context.Response.StatusCode);
		}

		[TestMethod]
		public async Task MiddlewareHandlesConflictException()
		{
			var context = new DefaultHttpContext();
			var errorMessage = "Test message";

			async Task next(HttpContext ctx)
			{
				throw new ConflictException(errorMessage);
			}

			context.Response.Body = new MemoryStream();

			await _middleware.InvokeAsync(context, next);

			Assert.AreEqual(StatusCodes.Status409Conflict, context.Response.StatusCode);
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
			var jsonResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody);

			Assert.AreEqual("Conflict Error", jsonResponse.Title);
			Assert.AreEqual(StatusCodes.Status409Conflict, jsonResponse.Status);
			Assert.AreEqual(errorMessage, jsonResponse.Detail);
		}

		[TestMethod]
		public async Task MiddlewareHandlesItemNotFoundException()
		{
			var context = new DefaultHttpContext();
			var id = 1;

			async Task next(HttpContext ctx)
			{
				throw new ItemNotFoundException(id);
			}

			context.Response.Body = new MemoryStream();

			await _middleware.InvokeAsync(context, next);

			Assert.AreEqual(StatusCodes.Status404NotFound, context.Response.StatusCode);
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
			var jsonResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody);

			Assert.AreEqual("Not Found", jsonResponse.Title);
			Assert.AreEqual(StatusCodes.Status404NotFound, jsonResponse.Status);
			Assert.AreEqual($"Item with the identifier {id} was not found.", jsonResponse.Detail);
		}

		[TestMethod]
		public async Task MiddlewareHandlesInvalidIdException()
		{
			var context = new DefaultHttpContext();

			async Task next(HttpContext ctx)
			{
				throw new InvalidIdException();
			}

			context.Response.Body = new MemoryStream();

			await _middleware.InvokeAsync(context, next);

			Assert.AreEqual(StatusCodes.Status404NotFound, context.Response.StatusCode);
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
			var jsonResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody);

			Assert.AreEqual("Not Found", jsonResponse.Title);
			Assert.AreEqual(StatusCodes.Status404NotFound, jsonResponse.Status);
			Assert.AreEqual("Id must be a positive number", jsonResponse.Detail);
		}

		[TestMethod]
		public async Task MiddlewareHandlesValidationsException()
		{
			var context = new DefaultHttpContext();
			var errorMessage = "Test message";

			async Task next(HttpContext ctx)
			{
				throw new ValidationsException(errorMessage);
			}

			context.Response.Body = new MemoryStream();

			await _middleware.InvokeAsync(context, next);

			Assert.AreEqual(StatusCodes.Status400BadRequest, context.Response.StatusCode);
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
			var jsonResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody);

			Assert.AreEqual("Validation Error", jsonResponse.Title);
			Assert.AreEqual(StatusCodes.Status400BadRequest, jsonResponse.Status);
			Assert.AreEqual(errorMessage, jsonResponse.Detail);
		}

		[TestMethod]
		public async Task MiddlewareHandlesException()
		{
			var context = new DefaultHttpContext();
			var errorMessage = "Test message";

			async Task next(HttpContext ctx)
			{
				throw new Exception(errorMessage);
			}

			context.Response.Body = new MemoryStream();

			await _middleware.InvokeAsync(context, next);

			Assert.AreEqual(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
			var jsonResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody);

			Assert.AreEqual("Server Error", jsonResponse.Title);
			Assert.AreEqual(StatusCodes.Status500InternalServerError, jsonResponse.Status);
			Assert.AreEqual(errorMessage, jsonResponse.Detail);
		}
	}
}
