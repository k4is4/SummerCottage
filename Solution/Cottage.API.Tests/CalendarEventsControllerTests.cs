using Cottage.API.Controllers;
using Cottage.API.Models;
using Cottage.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cottage.API.Tests
{
	[TestClass]
	public class CalendarEventsControllerTests
	{
		private Mock<ICalendarEventsService> _mockService;
		private CalendarEventsController _sut;

		[TestInitialize]
		public void TestInitialize()
		{
			_mockService = new Mock<ICalendarEventsService>();
			_sut = new CalendarEventsController(_mockService.Object);
		}

		[TestCleanup]
		public void TestCleanUp()
		{
			_sut = null;
			_mockService = null;
		}

		[TestMethod]
		public void Constructor_ServiceIsNull_ThrowsArgumentNullException()
		{
			// Act => Assert
			Assert.ThrowsException<ArgumentNullException>(() => new CalendarEventsController(null));
		}

		[TestMethod]
		public async Task GetCalendarEvents_ReturnsOkObjectResult()
		{
			// Arrange
			var eventsTestData = new List<CalendarEvent>();

			for (int i = 1; i <= 10; i++)
			{
				eventsTestData.Add(new CalendarEvent()
				{
					Id = i,
					StartDate = new DateTime(2023, 1, 1),
					EndDate = new DateTime(2023, 2, 2),
					Note = "test" + i.ToString(),
					Color = 1,
					UpdatedOn = new DateTime(2023, 3, 3)
				});
			}

			_mockService.Setup(m => m.GetCalendarEvents()).ReturnsAsync(eventsTestData);

			// Act
			var result = await _sut.GetCalendarEvents();

			// Assert
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
		}

		[TestMethod]
		[DataRow(1)]
		[DataRow(10)]
		[DataRow(100)]
		public async Task GetCalendarEvents_ReturnsSameContentAsTestData(int testDataSize)
		{
			// Arrange
			List<CalendarEvent> eventsTestData = new List<CalendarEvent>();
			for (int i = 1; i <= testDataSize; i++)
			{
				var eventItem = new CalendarEvent()
				{
					Id = i,
					StartDate = new DateTime(2023, 1, 1),
					EndDate = new DateTime(2023, 2, 2),
					Note = "test" + i.ToString(),
					Color = 1,
					UpdatedOn = new DateTime(2023, 3, 3)
				};
				eventsTestData.Add(eventItem);
			}

			_mockService.Setup(m => m.GetCalendarEvents()).ReturnsAsync(eventsTestData);

			// Act
			var result = await _sut.GetCalendarEvents();

			var response = result.Result as OkObjectResult;
			Assert.IsNotNull(response, "Response from GetCalendarEvents is null. Expected OkObjectResult.");

			var value = response.Value as List<CalendarEvent>;
			Assert.IsNotNull(value, "Failed to retrieve events from the response. Value is null.");

			// Assert
			if (value.Count != eventsTestData.Count())
			{
				Assert.Fail("GetCalendarEvents should return the same number of events as in test data");
			}

			for (int i = 0; i < value.Count; i++)
			{
				Assert.AreEqual(value[i], eventsTestData[i]);
			}
		}

		[TestMethod]
		public async Task GetCalendarEvents_0EventsInTestData_ReturnsEmptyList()
		{
			// Arrange
			_mockService.Setup(m => m.GetCalendarEvents()).ReturnsAsync(new List<CalendarEvent>());

			// Act
			var result = await _sut.GetCalendarEvents();

			var response = result.Result as OkObjectResult;
			Assert.IsNotNull(response, "Response from GetCalendarEvents is null. Expected OkObjectResult.");

			var value = response.Value as List<CalendarEvent>;
			Assert.IsNotNull(value, "Failed to retrieve empty list from the response. Value is null.");

			// Assert
			Assert.AreEqual(0, value.Count);
		}

		[TestMethod]
		public async Task GetCalendarEvents_CallsServiceOnce()
		{
			// Act
			await _sut.GetCalendarEvents();

			// Assert
			_mockService.Verify(m => m.GetCalendarEvents());
		}

		[TestMethod]
		public async Task GetCalendarEvents_ExceptionFromService_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("message");
			_mockService.Setup(m => m.GetCalendarEvents()).Throws(exception);

			// Act => Assert
			try
			{
				await _sut.GetCalendarEvents();
				Assert.Fail("Error from Service should have come through");
			}
			catch (Exception e)
			{
				Assert.AreEqual(e.Message, exception.Message);
			}
		}

		[TestMethod]
		public async Task GetCalendarEvent_Success_ReturnsOkObjectResult()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3)
			};
			_mockService.Setup(m => m.GetCalendarEvent(1)).ReturnsAsync(testEvent);

			// Act
			var result = await _sut.GetCalendarEvent(1);

			// Assert
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
		}

		[TestMethod]
		public async Task GetCalendarEvent_ReturnsSameContentAsTestData()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3)
			};

			_mockService.Setup(m => m.GetCalendarEvent(1)).ReturnsAsync(testEvent);

			// Act
			var result = await _sut.GetCalendarEvent(1);

			var response = result.Result as OkObjectResult;
			Assert.IsNotNull(response, "Response from GetCalendarEvent is null. Expected OkObjectResult.");

			var value = response.Value as CalendarEvent;
			Assert.IsNotNull(value, "Failed to retrieve calendar event from the response. Value is null.");

			// Assert
			Assert.AreEqual(value, testEvent);
		}

		[TestMethod]
		public async Task GetCalendarEvent_ExceptionFromService_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("message");
			_mockService.Setup(m => m.GetCalendarEvent(It.IsAny<int>())).Throws(exception);

			// Act => Assert
			try
			{
				await _sut.GetCalendarEvent(1);
				Assert.Fail("Error from Service should have come through");
			}
			catch (Exception e)
			{
				Assert.AreEqual(e.Message, exception.Message);
			}
		}

		[TestMethod]
		public async Task GetCalendarEvent_CallsServiceOnce()
		{
			// Act
			await _sut.GetCalendarEvent(1);

			// Assert
			_mockService.Verify(m => m.GetCalendarEvent(1));
		}

		[TestMethod]
		public async Task PutCalendarEvent_Success_ReturnsOkObjectResult()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3)			
			};

			_mockService.Setup(m => m.UpdateCalendarEvent(It.IsAny<int>(), It.IsAny<CalendarEvent>()));

			// Act
			var result = await _sut.PutCalendarEvent(1, testEvent);

			// Assert
			Assert.IsInstanceOfType(result, typeof(OkObjectResult));
		}

		[TestMethod]
		public async Task PutCalendarEvent_ReturnsSameContentAsTestData()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3)
			};

			_mockService.Setup(m => m.UpdateCalendarEvent(1, testEvent)).ReturnsAsync(testEvent);

			// Act
			var result = await _sut.PutCalendarEvent(1, testEvent);

			var response = result as OkObjectResult;
			Assert.IsNotNull(response, "Response from PutCalendarEvent is null. Expected OkObjectResult.");

			var value = response.Value as CalendarEvent;
			Assert.IsNotNull(value, "Failed to retrieve calendar event from the response. Value is null.");

			// Assert
			Assert.AreEqual(value, testEvent);
		}

		[TestMethod]
		public async Task PutCalendarEvent_Success_ReturnsTypeCalendarEvent()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3)
			};

			_mockService.Setup(m => m.UpdateCalendarEvent(It.IsAny<int>(), It.IsAny<CalendarEvent>())).Returns(Task.FromResult(testEvent));

			// Act
			var result = await _sut.PutCalendarEvent(1, testEvent);

			var response = result as OkObjectResult;
			Assert.IsNotNull(response, "Response from PutCalendarEvent is null. Expected OkObjectResult.");

			var value = response.Value as CalendarEvent;
			Assert.IsNotNull(value, "Failed to retrieve calendar event from the response. Value is null.");

			Assert.IsInstanceOfType(value, typeof(CalendarEvent));
		}

		[TestMethod]
		public async Task PutCalendarEvent_CallsServiceOnce()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3)
			};

			// Act
			await _sut.PutCalendarEvent(1, testEvent);

			// Assert
			_mockService.Verify(m => m.UpdateCalendarEvent(1, testEvent));
		}

		[TestMethod]
		public async Task PutCalendarEvent_ExceptionFromService_ThrowsSameException()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3)
			};

			var exception = new Exception("message");
			_mockService.Setup(m => m.UpdateCalendarEvent(It.IsAny<int>(), It.IsAny<CalendarEvent>())).Throws(exception);

			// Act => Assert
			try
			{
				await _sut.PutCalendarEvent(1, testEvent);
				Assert.Fail("Error from Service should have come through");
			}
			catch (Exception e)
			{
				Assert.AreEqual(e.Message, exception.Message);
			}
		}

		[TestMethod]
		public async Task PostCalendarEvent_CallsServiceOnce()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1
			};

			_mockService.Setup(m => m.AddCalendarEvent(It.IsAny<CalendarEvent>())).Returns(Task.FromResult(testEvent));

			// Act
			await _sut.PostCalendarEvent(testEvent);

			// Assert
			_mockService.Verify(m => m.AddCalendarEvent(testEvent), Times.Once);
		}

		[TestMethod]
		public async Task PostCalendarEvent_ExceptionFromService_ThrowsSameException()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1,
			};

			var exception = new Exception("message");
			_mockService.Setup(m => m.AddCalendarEvent(It.IsAny<CalendarEvent>())).Throws(exception);

			// Act => Assert
			try
			{
				await _sut.PostCalendarEvent(testEvent);
				Assert.Fail("Error from Service should have come through");
			}
			catch (Exception e)
			{
				Assert.AreEqual(e.Message, exception.Message);
			}
		}

		[TestMethod]
		public async Task PostCalendarEvent_Success_ReturnsCreatedAtAction()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1,
			};

			_mockService.Setup(m => m.AddCalendarEvent(It.IsAny<CalendarEvent>())).Returns(Task.FromResult(testEvent));

			// Act
			var result = await _sut.PostCalendarEvent(testEvent);

			// Assert
			Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
		}

		[TestMethod]
		public async Task PostCalendarEvent_ReturnsSameContentAsTestData()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3)
			};

			_mockService.Setup(m => m.AddCalendarEvent(testEvent)).ReturnsAsync(testEvent);

			// Act
			var result = await _sut.PostCalendarEvent(testEvent);

			var response = result.Result as CreatedAtActionResult;
			Assert.IsNotNull(response, "Response from PostCalendarEvent is null. Expected CreatedAtActionResult.");

			var value = response.Value as CalendarEvent;
			Assert.IsNotNull(value, "Failed to retrieve calendar event from the response. Value is null.");

			// Assert
			Assert.AreEqual(value, testEvent);
		}

		[TestMethod]
		public async Task PostCalendarEvent_Success_ReturnsTypeCalendarEvent()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3)
			};

			_mockService.Setup(m => m.AddCalendarEvent(It.IsAny<CalendarEvent>())).Returns(Task.FromResult(testEvent));

			// Act
			var result = await _sut.PostCalendarEvent(testEvent);

			var response = result.Result as CreatedAtActionResult;
			Assert.IsNotNull(response, "Response from PostCalendarEvent is null. Expected CreatedAtActionResult.");

			var value = response.Value as CalendarEvent;
			Assert.IsNotNull(value, "Failed to retrieve calendar event from the response. Value is null.");

			Assert.IsInstanceOfType(value, typeof(CalendarEvent));
		}

		[TestMethod]
		public async Task DeleteCalendarEvent_Success_ReturnsNoContent()
		{
			// Arrange
			_mockService.Setup(m => m.DeleteCalendarEvent(It.IsAny<int>())).ReturnsAsync(true);

			// Act
			var result = await _sut.DeleteCalendarEvent(1);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(NoContentResult));
		}

		[TestMethod]
		public async Task DeleteCalendarEvent_CallsServiceOnce()
		{
			// Act
			var result = await _sut.DeleteCalendarEvent(1);

			// Assert
			_mockService.Verify(m => m.DeleteCalendarEvent(1), Times.Once);
		}

		[TestMethod]
		public async Task DeleteCalendarEvent_ExceptionFromService_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("message");
			_mockService.Setup(m => m.DeleteCalendarEvent(It.IsAny<int>())).Throws(exception);

			// Act => Assert
			try
			{
				await _sut.DeleteCalendarEvent(1);
				Assert.Fail("Error from Service should have come through");
			}
			catch (Exception e)
			{
				Assert.AreEqual(e.Message, exception.Message);
			}
		}

	}
}