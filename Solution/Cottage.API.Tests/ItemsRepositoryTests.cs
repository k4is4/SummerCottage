using Cottage.API.Exceptions;
using Cottage.API.Models;
using Cottage.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cottage.API.Tests
{
	[TestClass]
	public class ItemsRepositoryTests
	{
		ItemsRepository _sut;
		CottageContext _dbContext;

		[TestInitialize]
		public async Task TestInitialize()
		{
			_dbContext = new TestDataDbContext().GetDatabaseContext();
			_sut = new ItemsRepository(_dbContext);
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
			Assert.ThrowsException<ArgumentNullException>(() => new ItemsRepository(null));
		}

		[TestMethod]
		[DataRow(1)]
		[DataRow(10)]
		[DataRow(100)]
		public async Task GetAll_ReturnsSameContentAsTestData(int testDataSize)
		{
			// Arrange
			List<Item> itemsTestData = new List<Item>();
			for (int i = 1; i <= testDataSize; i++)
			{
				var item = new Item()
				{
					Id = i,
					Name = "TestItem" + i.ToString(),
					Status = 1,
					Category = 1,
					UpdatedOn = new DateTime(2023, 1, 1),
				};
				itemsTestData.Add(item);
				_dbContext.Items.Add(item);
				await _dbContext.SaveChangesAsync();
			}

			// Act
			var result = await _sut.GetAll();

			// Assert
			CollectionAssert.AreEqual(itemsTestData, result.ToList());
		}

		[TestMethod]
		public async Task GetAll_ItemsTableEmpty_ReturnsEmptyList()
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
			dbContextMock.Setup(m => m.Items).Throws(exception);
			var sut = new ItemsRepository(dbContextMock.Object);


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
			dbContextMock.Setup(m => m.Items);
			var sut = new ItemsRepository(dbContextMock.Object);

			// Act
			sut.GetAll();

			// Assert
			dbContextMock.Verify(m => m.Items);
		}

		[TestMethod]
		public async Task GetById_Success_ReturnsExpectedItem()
		{
			// Arrange
			var testItem = new Item()
			{
				Id = 1,
				Name = "TestItem",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1),
			};
			_dbContext.Items.Add(testItem);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _sut.GetById(testItem.Id);

			// Assert
			Assert.AreEqual(testItem, result);
		}

		[TestMethod]
		public async Task GetById_ItemNotFound_ReturnsNull()
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
			dbContextMock.Setup(m => m.Items.FindAsync(1)).Throws(exception);
			var sut = new ItemsRepository(dbContextMock.Object);


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
			dbContextMock.Setup(m => m.Items.FindAsync(1));
			var sut = new ItemsRepository(dbContextMock.Object);

			// Act
			await sut.GetById(1);

			// Assert
			dbContextMock.Verify(m => m.Items.FindAsync(1));
		}

		[TestMethod]
		public async Task Update_Success_ReturnsExpectedItem()
		{
			// Arrange
			var testItem = new Item()
			{
				Id = 1,
				Name = "TestItem",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1),
			};

			var updatedTestItem = new Item()
			{
				Id = 1,
				Name = "Updated Name",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1),
			};
			_dbContext.Items.Add(testItem);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _sut.Update(updatedTestItem);

			// Assert
			Assert.AreEqual(updatedTestItem.Name, result.Name);
		}

		[TestMethod]
		public async Task Update_ItemNotFound_ReturnsNull()
		{
			// Arrange
			var testItem = new Item()
			{
				Id = 1,
				Name = "TestItem",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1),
			};

			// Act
			var result = await _sut.Update(testItem);

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task Update_ExceptionFromDbContext_ThrowsSameException()
		{
			// Arrange
			var testItem = new Item()
			{
				Id = 1,
				Name = "TestItem",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1),
			};

			var exception = new Exception("message");
			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.Items.FindAsync(1)).Throws(exception);
			var sut = new ItemsRepository(dbContextMock.Object);


			// Act => Assert
			Exception thrownException = null;
			try
			{
				await sut.Update(testItem);
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
			var testItem = new Item()
			{
				Id = 1,
				Name = "TestItem",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1),
			};

			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.Items.FindAsync(1));
			var sut = new ItemsRepository(dbContextMock.Object);

			// Act
			await sut.Update(testItem);

			// Assert
			dbContextMock.Verify(m => m.Items.FindAsync(1));
		}

		[TestMethod]
		public async Task Add_Success_ReturnsExpectedItem()
		{
			// Arrange
			var testItem = new Item()
			{
				Name = "TestItem",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1),
			};

			// Act
			var result = await _sut.Add(testItem);

			// Assert
			Assert.AreEqual(testItem.Name, result.Name);
		}

		[TestMethod]
		public async Task Add_ExceptionFromDbContext_ThrowsSameException()
		{
			// Arrange
			var testItem = new Item()
			{
				Name = "TestItem",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1),
			};

			var exception = new Exception("message");
			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.Items.Add(testItem)).Throws(exception);
			var sut = new ItemsRepository(dbContextMock.Object);


			// Act => Assert
			Exception thrownException = null;
			try
			{
				await sut.Add(testItem);
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
			var testItem = new Item()
			{
				Name = "TestItem",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1),
			};

			var dbContextMock = new Mock<CottageContext>();
			dbContextMock.Setup(m => m.Items.Add(testItem));
			var sut = new ItemsRepository(dbContextMock.Object);

			// Act
			await sut.Add(testItem);

			// Assert
			dbContextMock.Verify(m => m.Items.Add(testItem));
		}

		[TestMethod]
		public async Task Delete_Success_ReturnsTrue()
		{
			// Arrange
			var testItem = new Item()
			{
				Id = 1,
				Name = "TestItem",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1),
			};
			_dbContext.Items.Add(testItem);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _sut.Delete(testItem.Id);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public async Task Delete_ItemNotFound_ReturnsFalse()
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
			dbContextMock.Setup(m => m.Items.FindAsync(1)).Throws(exception);
			var sut = new ItemsRepository(dbContextMock.Object);


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
			dbContextMock.Setup(m => m.Items.FindAsync(1));
			var sut = new ItemsRepository(dbContextMock.Object);

			// Act
			await sut.Delete(1);

			// Assert
			dbContextMock.Verify(m => m.Items.FindAsync(1));
		}

		[TestMethod]
		public async Task DoesNameExistAsync_ItemNameExists_ReturnsTrue()
		{
			// Arrange
			string existingName = "TestItem";
			var testItem = new Item()
			{
				Name = existingName,
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1)
			};
			_dbContext.Items.Add(testItem);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _sut.DoesNameExistAsync(existingName);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public async Task DoesNameExistAsync_ItemNameDoesNotExist_ReturnsFalse()
		{
			// Act
			var result = await _sut.DoesNameExistAsync("NonExistingName");

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public async Task DoesNameExistAsync_ItemNameExistsButExcludedIdMatches_ReturnsFalse()
		{
			// Arrange
			string existingName = "TestItem";
			var testItem = new Item()
			{
				Name = existingName,
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1)
			};
			_dbContext.Items.Add(testItem);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _sut.DoesNameExistAsync(existingName, testItem.Id);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public async Task DoesNameExistAsync_ItemNameExistsButDifferentId_ReturnsTrue()
		{
			// Arrange
			string existingName = "TestItem";
			var testItem1 = new Item()
			{
				Name = existingName,
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1)
			};
			var testItem2 = new Item()
			{
				Name = "OtherItem",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1)
			};
			_dbContext.Items.Add(testItem1);
			_dbContext.Items.Add(testItem2);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _sut.DoesNameExistAsync(existingName, testItem2.Id);

			// Assert
			Assert.IsTrue(result);
		}
	}
	}
