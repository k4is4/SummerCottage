using Cottage.API.Models;
using Cottage.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cottage.API.Tests
{
	[TestClass]
	public class CalendarEventsRepositoryTests
	{
		CalendarEventsRepository _sut;
		CottageContext _dbContext;

		[TestInitialize]
		public async Task TestInitialize()
		{
			_dbContext = new TestDataDbContext().GetDatabaseContext();
			_sut = new CalendarEventsRepository(_dbContext);
		}

		[TestCleanup]
		public void TestCleanUp()
		{
			_sut = null;
			_dbContext = null;
		}

		[TestMethod]
		public void Constructor_DbContextIsNull_ThrowsArgumentNullException()
		{
			// Act => Assert
			Assert.ThrowsException<ArgumentNullException>(() => new CalendarEventsRepository(null));
		}

		[TestMethod]
		[DataRow(1)]
		[DataRow(10)]
		[DataRow(100)]
		public async Task GetAll_ReturnsSameContentAsTestData(int testDataSize)
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
				_dbContext.CalendarEvents.Add(calendarEvent);
				await _dbContext.SaveChangesAsync();
			}

			// Act
			var result = await _sut.GetAll();

			// Assert
			CollectionAssert.AreEqual(eventsTestData, result.ToList());
		}

		[TestMethod]
		public async Task GetAll_CalendarEventsTableEmpty_ReturnsEmptyList()
		{
			// Act
			var result = await _sut.GetAll();

			// Assert
			Assert.IsNotNull(result, "Response from GetAll is null. Expected an empty list.");
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void GetAll_ExceptionFromDbContext_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("message");
			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.CalendarEvents).Throws(exception);
			var sut = new CalendarEventsRepository(dbContextMock.Object);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				sut.GetAll();
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public void GetAll_CallsDbContextOnce()
		{
			// Arrange
			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.CalendarEvents);
			var sut = new CalendarEventsRepository(dbContextMock.Object);

			// Act
			sut.GetAll();

			// Assert
			dbContextMock.Verify(m => m.CalendarEvents);
		}

		[TestMethod]
		public async Task GetById_Success_ReturnsExpectedCalendarEvent()
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
			_dbContext.CalendarEvents.Add(testEvent);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _sut.GetById(testEvent.Id);

			// Assert
			Assert.AreEqual(testEvent, result);
		}

		[TestMethod]
		public async Task GetById_CalendarEventNotFound_ReturnsNull()
		{
			// Act
			var result = await _sut.GetById(1);

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task GetById_ExceptionFromDbContext_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("message");
			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.CalendarEvents.FindAsync(1)).Throws(exception);
			var sut = new CalendarEventsRepository(dbContextMock.Object);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await sut.GetById(1);
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task GetById_CallsDbContextOnce()
		{
			// Arrange
			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.CalendarEvents.FindAsync(1));
			var sut = new CalendarEventsRepository(dbContextMock.Object);

			// Act
			await sut.GetById(1);

			// Assert
			dbContextMock.Verify(m => m.CalendarEvents.FindAsync(1));
		}

		[TestMethod]
		public async Task Update_Success_ReturnsExpectedCalendarEvent()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 1, 2),
				Note = "TestNote",
				Color = 1,
				UpdatedOn = new DateTime(2023, 1, 3),
			};

			var updatedTestEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 10),
				EndDate = new DateTime(2023, 1, 12),
				Note = "Updated Note",
				Color = 2,
				UpdatedOn = new DateTime(2023, 1, 13),
			};
			_dbContext.CalendarEvents.Add(testEvent);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _sut.Update(updatedTestEvent);

			// Assert
			Assert.AreEqual(updatedTestEvent.Note, result.Note);
			Assert.AreEqual(updatedTestEvent.StartDate, result.StartDate);
			Assert.AreEqual(updatedTestEvent.EndDate, result.EndDate);
			Assert.AreEqual(updatedTestEvent.Color, result.Color);
		}

		[TestMethod]
		public async Task Update_CalendarEventNotFound_ReturnsNull()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 1, 2),
				Note = "TestNote",
				Color = 1,
				UpdatedOn = new DateTime(2023, 1, 3),
			};

			// Act
			var result = await _sut.Update(testEvent);

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task Update_ExceptionFromDbContext_ThrowsSameException()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 1, 2),
				Note = "TestNote",
				Color = 1,
				UpdatedOn = new DateTime(2023, 1, 3),
			};

			var exception = new Exception("message");
			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.CalendarEvents.FindAsync(1)).Throws(exception);
			var sut = new CalendarEventsRepository(dbContextMock.Object);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await sut.Update(testEvent);
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task Update_CallsDbContextOnce()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 1, 2),
				Note = "TestNote",
				Color = 1,
				UpdatedOn = new DateTime(2023, 1, 3),
			};

			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.CalendarEvents.FindAsync(1));
			var sut = new CalendarEventsRepository(dbContextMock.Object);

			// Act
			await sut.Update(testEvent);

			// Assert
			dbContextMock.Verify(m => m.CalendarEvents.FindAsync(1));
		}

		[TestMethod]
		public async Task Add_Success_ReturnsExpectedCalendarEvent()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 1, 2),
				Note = "TestNote",
				Color = 1,
			};

			// Act
			var result = await _sut.Add(testEvent);

			// Assert
			Assert.AreEqual(testEvent.Note, result.Note);
			Assert.AreEqual(testEvent.StartDate, result.StartDate);
			Assert.AreEqual(testEvent.EndDate, result.EndDate);
			Assert.AreEqual(testEvent.Color, result.Color);
		}

		[TestMethod]
		public async Task Add_ExceptionFromDbContext_ThrowsSameException()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 1, 2),
				Note = "TestNote",
				Color = 1,
			};

			var exception = new Exception("message");
			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.CalendarEvents.Add(testEvent)).Throws(exception);
			var sut = new CalendarEventsRepository(dbContextMock.Object);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await sut.Add(testEvent);
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task Add_CallsDbContextOnce()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 1, 2),
				Note = "TestNote",
				Color = 1,
			};

			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.CalendarEvents.Add(testEvent));
			var sut = new CalendarEventsRepository(dbContextMock.Object);

			// Act
			await sut.Add(testEvent);

			// Assert
			dbContextMock.Verify(m => m.CalendarEvents.Add(testEvent));
		}

		[TestMethod]
		public async Task Delete_Success_ReturnsTrue()
		{
			// Arrange
			var testEvent = new CalendarEvent()
			{
				Id = 1,
				StartDate = new DateTime(2023, 1, 1),
				EndDate = new DateTime(2023, 1, 2),
				Note = "TestNote",
				Color = 1,
				UpdatedOn = new DateTime(2023, 1, 3),
			};
			_dbContext.CalendarEvents.Add(testEvent);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _sut.Delete(testEvent.Id);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public async Task Delete_EventNotFound_ReturnsFalse()
		{
			// Act
			var result = await _sut.Delete(1);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public async Task Delete_ExceptionFromDbContext_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("message");
			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.CalendarEvents.FindAsync(1)).Throws(exception);
			var sut = new CalendarEventsRepository(dbContextMock.Object);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await sut.Delete(1);
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task Delete_CallsDbContextOnce()
		{
			// Arrange
			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.CalendarEvents.FindAsync(1));
			var sut = new CalendarEventsRepository(dbContextMock.Object);

			// Act
			await sut.Delete(1);

			// Assert
			dbContextMock.Verify(m => m.CalendarEvents.FindAsync(1));
		}
	}
}
