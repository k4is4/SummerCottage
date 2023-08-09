using Cottage.API.Exceptions;
using Cottage.API.Models;
using Cottage.API.Repositories;
using Cottage.API.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cottage.API.Tests
{
	[TestClass]
	public class CalendarEventsServiceTests
	{
		private Mock<ICalendarEventsRepository> _mockRepository;
		private CalendarEventsService _sut;

		[TestInitialize]
		public void TestInitialize()
		{
			_mockRepository = new Mock<ICalendarEventsRepository>();
			_sut = new CalendarEventsService(_mockRepository.Object);
		}

		[TestCleanup]
		public void TestCleanUp()
		{
			_sut = null;
			_mockRepository = null;
		}

		[TestMethod]
		public void Constructor_RepositoryIsNull_ThrowsArgumentNullException()
		{
			// Act => Assert
			Assert.ThrowsException<ArgumentNullException>(() => new CalendarEventsService(null));
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
				var calendarEvent = new CalendarEvent()
				{
					Id = i,
					StartDate = new DateTime(2023, 1, 1),
					EndDate = new DateTime(2023, 2, 2),
					Note = "test" + i.ToString(),
					Color = 1,
					UpdatedOn = new DateTime(2023, 3, 3)
				};
				eventsTestData.Add(calendarEvent);
			}

			_mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(eventsTestData);

			// Act
			var result = await _sut.GetCalendarEvents();

			// Assert
			CollectionAssert.AreEqual(eventsTestData, result.ToList());
		}

		[TestMethod]
		public async Task GetCalendarEvents_0EventsInTestData_ReturnsEmptyList()
		{
			// Arrange
			_mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<CalendarEvent>());

			// Act
			var result = await _sut.GetCalendarEvents();

			// Assert
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public async Task GetCalendarEvents_CallsRepositoryOnce()
		{
			// Act
			await _sut.GetCalendarEvents();

			// Assert
			_mockRepository.Verify(repo => repo.GetAll(), Times.Once);
		}

		[TestMethod]
		public async Task GetCalendarEvents_ExceptionFromRepository_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("Test exception message");
			_mockRepository.Setup(repo => repo.GetAll()).Throws(exception);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await _sut.GetCalendarEvents();
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task GetCalendarEvent_Success_ReturnsExpectedEvent()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "Test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3),
			};

			_mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(testEvent);

			// Act
			var result = await _sut.GetCalendarEvent(1);

			// Assert
			Assert.AreEqual(testEvent, result);
		}

		[TestMethod]
		public async Task GetCalendarEvent_InvalidId_ThrowsInvalidIdException()
		{
			// Act => Assert
			await Assert.ThrowsExceptionAsync<InvalidIdException>(() => _sut.GetCalendarEvent(0));
		}

		[TestMethod]
		public async Task GetCalendarEvent_NotFound_ThrowsItemNotFoundException()
		{
			// Arrange
			_mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync((CalendarEvent)null);

			// Act => Assert
			await Assert.ThrowsExceptionAsync<ItemNotFoundException>(() => _sut.GetCalendarEvent(1));
		}

		[TestMethod]
		public async Task GetCalendarEvent_CallsRepositoryOnce()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "Test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3),
			};

			_mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(testEvent);

			// Act
			await _sut.GetCalendarEvent(1);

			// Assert
			_mockRepository.Verify(repo => repo.GetById(1), Times.Once);
		}

		[TestMethod]
		public async Task GetCalendarEvent_ExceptionFromRepository_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("Test exception message");
			_mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Throws(exception);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await _sut.GetCalendarEvent(1);
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task UpdateCalendarEvent_InvalidId_ThrowsInvalidIdException()
		{
			var calendarEvent = new CalendarEvent { Id = -1 };

			await Assert.ThrowsExceptionAsync<InvalidIdException>(() => _sut.UpdateCalendarEvent(-1, calendarEvent));
		}

		[TestMethod]
		public async Task UpdateCalendarEvent_MismatchedIds_ThrowsValidationsException()
		{
			var calendarEvent = new CalendarEvent { Id = 2 };

			await Assert.ThrowsExceptionAsync<ValidationsException>(() => _sut.UpdateCalendarEvent(1, calendarEvent));
		}

		[TestMethod]
		public async Task UpdateCalendarEvent_SuccessfulUpdate_ReturnsUpdatedEvent()
		{
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "Test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3),
			};

			_mockRepository.Setup(r => r.Update(testEvent)).ReturnsAsync(testEvent);

			var result = await _sut.UpdateCalendarEvent(1, testEvent);

			Assert.AreEqual(testEvent, result);
		}

		[TestMethod]
		public async Task UpdateCalendarEvent_EventNotFound_ThrowsItemNotFoundException()
		{
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "Test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3),
			};
			_mockRepository.Setup(r => r.Update(testEvent)).ReturnsAsync((CalendarEvent)null);

			await Assert.ThrowsExceptionAsync<ItemNotFoundException>(() => _sut.UpdateCalendarEvent(1, testEvent));
		}

		[TestMethod]
		public async Task UpdateCalendarEvent_ExceptionFromRepository_ThrowsSameException()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "Test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3),
			};
			var exception = new Exception("Test exception message");
			_mockRepository.Setup(repo => repo.Update(testEvent)).Throws(exception);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await _sut.UpdateCalendarEvent(1, testEvent);
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task UpdateCalendarEvent_CallsRepositoryOnce()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "Test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3),
			};

			_mockRepository.Setup(repo => repo.Update(testEvent)).ReturnsAsync(testEvent);

			// Act
			await _sut.UpdateCalendarEvent(testEvent.Id, testEvent);

			// Assert
			_mockRepository.Verify(repo => repo.Update(testEvent), Times.Once);
		}

		[TestMethod]
		public async Task AddCalendarEvent_SuccessfulAdd_ReturnsAddedEvent()
		{
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "Test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3),
			};

			_mockRepository.Setup(r => r.Add(testEvent)).ReturnsAsync(testEvent);

			var result = await _sut.AddCalendarEvent(testEvent);

			Assert.AreEqual(testEvent, result);
		}

		[TestMethod]
		public async Task AddCalendarEvent_CallsRepositoryAddOnce()
		{
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "Test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3),
			};

			_mockRepository.Setup(r => r.Add(testEvent)).ReturnsAsync(testEvent);

			await _sut.AddCalendarEvent(testEvent);

			_mockRepository.Verify(m => m.Add(testEvent), Times.Once);
		}

		[TestMethod]
		public async Task AddCalendarEvent_ExceptionFromRepository_ThrowsSameException()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 2, 2),
				Note = "Test",
				Color = 1,
				UpdatedOn = new DateTime(2023, 3, 3),
			};
			var exception = new Exception("Test exception message");
			_mockRepository.Setup(repo => repo.Add(testEvent)).Throws(exception);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await _sut.AddCalendarEvent(testEvent);
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task DeleteCalendarEvent_SuccessfulDelete_ReturnsTrue()
		{
			int validId = 1;

			_mockRepository.Setup(r => r.Delete(validId)).ReturnsAsync(true);

			var result = await _sut.DeleteCalendarEvent(validId);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public async Task DeleteCalendarEvent_InvalidId_ThrowsInvalidIdException()
		{
			int invalidId = 0;

			await Assert.ThrowsExceptionAsync<InvalidIdException>(() => _sut.DeleteCalendarEvent(invalidId));
		}

		[TestMethod]
		public async Task DeleteCalendarEvent_EventNotFound_ThrowsItemNotFoundException()
		{
			int nonExistentId = 2;

			_mockRepository.Setup(r => r.Delete(nonExistentId)).ReturnsAsync(false);

			await Assert.ThrowsExceptionAsync<ItemNotFoundException>(() => _sut.DeleteCalendarEvent(nonExistentId));
		}

		[TestMethod]
		public async Task DeleteCalendarEvent_CallsRepositoryDeleteOnce()
		{
			int validId = 1;

			_mockRepository.Setup(r => r.Delete(validId)).ReturnsAsync(true);

			await _sut.DeleteCalendarEvent(validId);

			_mockRepository.Verify(m => m.Delete(validId), Times.Once);
		}

		[TestMethod]
		public async Task DeleteCalendarEvent_ExceptionFromRepository_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("Test exception message");
			_mockRepository.Setup(repo => repo.Delete(1)).Throws(exception);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await _sut.DeleteCalendarEvent(1);
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}
	}
}