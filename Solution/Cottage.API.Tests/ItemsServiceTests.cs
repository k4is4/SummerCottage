using Cottage.API.Controllers;
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
	public class ItemsServiceTests
	{
		private Mock<IItemsRepository> _mockRepository;
		private ItemsService _sut;

		[TestInitialize]
		public void TestInitialize()
		{
			_mockRepository = new Mock<IItemsRepository>();
			_sut = new ItemsService(_mockRepository.Object);
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
			Assert.ThrowsException<ArgumentNullException>(() => new ItemsService(null));
		}

		[TestMethod]
		[DataRow(1)]
		[DataRow(10)]
		[DataRow(100)]
		public async Task GetItems_ReturnsSameContentAsTestData(int testDataSize)
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
			}

			_mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(itemsTestData);

			// Act
			var result = await _sut.GetItems();

			// Assert
			CollectionAssert.AreEqual(itemsTestData, result.ToList());
		}

		[TestMethod]
		public async Task GetItems_0ItemsInTestData_ReturnsEmptyList()
		{
			// Arrange
			_mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Item>());

			// Act
			var result = await _sut.GetItems();

			// Assert
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public async Task GetItems_CallsRepositoryOnce()
		{
			// Act
			await _sut.GetItems();

			// Assert
			_mockRepository.Verify(repo => repo.GetAll(), Times.Once);
		}

		[TestMethod]
		public async Task GetItems_ExceptionFromRepository_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("Test exception message");
			_mockRepository.Setup(repo => repo.GetAll()).Throws(exception);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await _sut.GetItems();
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task GetItem_Success_ReturnsExpectedItem()
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

			_mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(testItem);

			// Act
			var result = await _sut.GetItem(1);

			// Assert
			Assert.AreEqual(testItem, result);
		}

		[TestMethod]
		public async Task GetItem_InvalidId_ThrowsInvalidIdException()
		{
			// Act => Assert
			await Assert.ThrowsExceptionAsync<InvalidIdException>(() => _sut.GetItem(0));
		}

		[TestMethod]
		public async Task GetItem_NotFound_ThrowsItemNotFoundException()
		{
			// Arrange
			_mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync((Item)null);

			// Act => Assert
			await Assert.ThrowsExceptionAsync<ItemNotFoundException>(() => _sut.GetItem(1));
		}

		[TestMethod]
		public async Task GetItem_CallsRepositoryOnce()
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

			_mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(testItem);

			// Act
			await _sut.GetItem(1);

			// Assert
			_mockRepository.Verify(repo => repo.GetById(1), Times.Once);
		}

		[TestMethod]
		public async Task GetItem_ExceptionFromRepository_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("Test exception message");
			_mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Throws(exception);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await _sut.GetItem(1);
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task UpdateItem_InvalidId_ThrowsInvalidIdException()
		{
			var item = new Item { Id = -1 };

			await Assert.ThrowsExceptionAsync<InvalidIdException>(() => _sut.UpdateItem(-1, item));
		}

		[TestMethod]
		public async Task UpdateItem_MismatchedIds_ThrowsValidationsException()
		{
			var item = new Item { Id = 2 };

			await Assert.ThrowsExceptionAsync<ValidationsException>(() => _sut.UpdateItem(1, item));
		}

		[TestMethod]
		public async Task UpdateItem_NameExists_ThrowsConflictException()
		{
			var item = new Item { Id = 1, Name = "TestItem" };
			_mockRepository.Setup(r => r.DoesNameExistAsync(item.Name, item.Id)).ReturnsAsync(true);

			await Assert.ThrowsExceptionAsync<ConflictException>(() => _sut.UpdateItem(1, item));
		}

		[TestMethod]
		public async Task UpdateItem_SuccessfulUpdate_ReturnsUpdatedItem()
		{
			var testItem = new Item()
			{
				Id = 1,
				Name = "TestItem",
				Status = 1,
				Category = 1,
				UpdatedOn = new DateTime(2023, 1, 1),
			};

			_mockRepository.Setup(r => r.DoesNameExistAsync(testItem.Name, testItem.Id)).ReturnsAsync(false);
			_mockRepository.Setup(r => r.Update(testItem)).ReturnsAsync(testItem);

			var result = await _sut.UpdateItem(1, testItem);

			Assert.AreEqual(testItem, result);
		}

		[TestMethod]
		public async Task UpdateItem_ItemNotFound_ThrowsItemNotFoundException()
		{
			var item = new Item { Id = 1, Name = "TestItem" };
			_mockRepository.Setup(r => r.DoesNameExistAsync(item.Name, item.Id)).ReturnsAsync(false);
			_mockRepository.Setup(r => r.Update(item)).ReturnsAsync((Item)null);

			await Assert.ThrowsExceptionAsync<ItemNotFoundException>(() => _sut.UpdateItem(1, item));
		}

		[TestMethod]
		public async Task UpdateItem_ExceptionFromRepository_ThrowsSameException()
		{
			// Arrange
			var item = new Item { Id = 1, Name = "TestItem" };
			var exception = new Exception("Test exception message");
			_mockRepository.Setup(repo => repo.Update(item)).Throws(exception);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await _sut.UpdateItem(1, item);
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task AddItem_SuccessfulAdd_ReturnsAddedItem()
		{
			var testItem = new Item()
			{
				Name = "TestItem",
				Status = 1,
				Category = 1
			};

			_mockRepository.Setup(r => r.DoesNameExistAsync(testItem.Name, null)).ReturnsAsync(false);
			_mockRepository.Setup(r => r.Add(testItem)).ReturnsAsync(testItem);

			var result = await _sut.AddItem(testItem);

			Assert.AreEqual(testItem, result);
		}

		[TestMethod]
		public async Task AddItem_NameExists_ThrowsConflictException()
		{
			var testItem = new Item()
			{
				Name = "TestItem",
				Status = 1,
				Category = 1
			};

			_mockRepository.Setup(r => r.DoesNameExistAsync(testItem.Name, null)).ReturnsAsync(true);

			await Assert.ThrowsExceptionAsync<ConflictException>(() => _sut.AddItem(testItem));
		}

		[TestMethod]
		public async Task AddItem_CallsRepositoryAddOnce()
		{
			var testItem = new Item()
			{
				Name = "TestItem",
				Status = 1,
				Category = 1
			};

			_mockRepository.Setup(r => r.DoesNameExistAsync(testItem.Name, null)).ReturnsAsync(false);
			_mockRepository.Setup(r => r.Add(testItem)).ReturnsAsync(testItem);

			await _sut.AddItem(testItem);

			_mockRepository.Verify(m => m.Add(testItem), Times.Once);
		}

		[TestMethod]
		public async Task AddItem_ExceptionFromRepository_ThrowsSameException()
		{
			// Arrange
			var testItem = new Item()
			{
				Name = "TestItem",
				Status = 1,
				Category = 1
			};
			var exception = new Exception("Test exception message");
			_mockRepository.Setup(repo => repo.Add(testItem)).Throws(exception);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await _sut.AddItem(testItem);
			}
			catch (Exception e)
			{
				thrownException = e;
			}

			Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
			Assert.AreEqual(exception.Message, thrownException.Message);
		}

		[TestMethod]
		public async Task DeleteItem_SuccessfulDelete_ReturnsTrue()
		{
			int validId = 1;

			_mockRepository.Setup(r => r.Delete(validId)).ReturnsAsync(true);

			var result = await _sut.DeleteItem(validId);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public async Task DeleteItem_InvalidId_ThrowsInvalidIdException()
		{
			int invalidId = 0;

			await Assert.ThrowsExceptionAsync<InvalidIdException>(() => _sut.DeleteItem(invalidId));
		}

		[TestMethod]
		public async Task DeleteItem_ItemNotFound_ThrowsItemNotFoundException()
		{
			int nonExistentId = 2;

			_mockRepository.Setup(r => r.Delete(nonExistentId)).ReturnsAsync(false);

			await Assert.ThrowsExceptionAsync<ItemNotFoundException>(() => _sut.DeleteItem(nonExistentId));
		}

		[TestMethod]
		public async Task DeleteItem_CallsRepositoryDeleteOnce()
		{
			int validId = 1;

			_mockRepository.Setup(r => r.Delete(validId)).ReturnsAsync(true);

			await _sut.DeleteItem(validId);

			_mockRepository.Verify(m => m.Delete(validId), Times.Once);
		}

		[TestMethod]
		public async Task DeleteItem_ExceptionFromRepository_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("Test exception message");
			_mockRepository.Setup(repo => repo.Delete(1)).Throws(exception);

			// Act => Assert
			Exception thrownException = null;
			try
			{
				await _sut.DeleteItem(1);
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
