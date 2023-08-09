using Cottage.API.Controllers;
using Cottage.API.Models;
using Cottage.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Cottage.API.Tests
{
	[TestClass]
	public class ItemsControllerTests
	{
		private Mock<IItemsService> _mockService;
		private ItemsController _sut;

		[TestInitialize]
		public void TestInitialize()
		{
			_mockService = new Mock<IItemsService>();
			_sut = new ItemsController(_mockService.Object);
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
			Assert.ThrowsException<ArgumentNullException>(() => new ItemsController(null));
		}

		[TestMethod]
		public async Task GetItems_ReturnsOkObjectResult()
		{
			// Arrange
			var itemsTestData = new List<Item>();

			for (int i = 1; i <= 10; i++)
			{
				itemsTestData.Add(new Item()
				{
					Id = i,
					Name = "TestItem" + i.ToString(),
					Status = 1,
					Category = 1,
					UpdatedOn = new DateTime(2023, 1, 1),
				});
			}

			_mockService.Setup(m => m.GetItems()).ReturnsAsync(itemsTestData);

			// Act
			var result = await _sut.GetItems();

			// Assert
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
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

			_mockService.Setup(m => m.GetItems()).ReturnsAsync(itemsTestData);

			// Act
			var result = await _sut.GetItems();

			var response = result.Result as OkObjectResult;
			Assert.IsNotNull(response, "Response from GetItems is null. Expected OkObjectResult.");

			var value = response.Value as List<Item>;
			Assert.IsNotNull(value, "Failed to retrieve items from the response. Value is null.");

			// Assert
			if (value.Count != itemsTestData.Count())
			{
				Assert.Fail("GetItems should return the same number of items as in test data");
			}

			for (int i = 0; i < value.Count; i++)
			{
				Assert.AreEqual(value[i], itemsTestData[i]);
			}
		}

		[TestMethod]
		public async Task GetItems_0ItemsInTestData_ReturnsEmptyList()
		{
			// Arrange
			_mockService.Setup(m => m.GetItems()).ReturnsAsync(new List<Item>());

			// Act
			var result = await _sut.GetItems();

			var response = result.Result as OkObjectResult;
			Assert.IsNotNull(response, "Response from GetItems is null. Expected OkObjectResult.");

			var value = response.Value as List<Item>;
			Assert.IsNotNull(value, "Failed to retrieve empty list from the response. Value is null.");

			// Assert
			Assert.AreEqual(0, value.Count);
		}

		[TestMethod]
		public async Task GetItems_CallsServiceOnce()
		{
			// Act
			await _sut.GetItems();

			// Assert
			_mockService.Verify(m => m.GetItems());
		}

		[TestMethod]
		public async Task GetItems_ExceptionFromService_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("message");
			_mockService.Setup(m => m.GetItems()).Throws(exception);

			// Act => Assert
			try
			{
				await _sut.GetItems();
				Assert.Fail("Error from Service should have come through");
			}
			catch (Exception e)
			{
				Assert.AreEqual(e.Message, exception.Message);
			}
		}

		[TestMethod]
		public async Task GetItem_Success_ReturnsOkObjectResult()
		{
			// Act
			var result = await _sut.GetItem(1);

			// Assert
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
		}

		[TestMethod]
		public async Task GetItem_ReturnsSameContentAsTestData()
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

			_mockService.Setup(m => m.GetItem(1)).ReturnsAsync(testItem);

			// Act
			var result = await _sut.GetItem(1);

			var response = result.Result as OkObjectResult;
			Assert.IsNotNull(response, "Response from GetItem is null. Expected OkObjectResult.");

			var value = response.Value as Item;
			Assert.IsNotNull(value, "Failed to retrieve items from the response. Value is null.");

			// Assert
			Assert.AreEqual(value, testItem);
		}

		[TestMethod]
		public async Task GetItem_ExceptionFromService_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("message");
			_mockService.Setup(m => m.GetItem(It.IsAny<int>())).Throws(exception);

			// Act => Assert
			try
			{
				await _sut.GetItem(1);
				Assert.Fail("Error from Service should have come through");
			}
			catch (Exception e)
			{
				Assert.AreEqual(e.Message, exception.Message);
			}
		}

		[TestMethod]
		public async Task GetItem_CallsServiceOnce()
		{
			// Act
			await _sut.GetItem(1);

			// Assert
			_mockService.Verify(m => m.GetItem(1));
		}

		[TestMethod]
		public async Task PutItem_Success_ReturnsOkObjectResult()
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

			_mockService.Setup(m => m.UpdateItem(It.IsAny<int>(), It.IsAny<Item>()));

			// Act
			var result = await _sut.PutItem(1, testItem);

			// Assert
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
		}

		[TestMethod]
		public async Task PutItem_ReturnsSameContentAsTestData()
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

			_mockService.Setup(m => m.UpdateItem(1, testItem)).ReturnsAsync(testItem);

			// Act
			var result = await _sut.PutItem(1, testItem);

			var response = result.Result as OkObjectResult;
			Assert.IsNotNull(response, "Response from PutItem is null. Expected OkObjectResult.");

			var value = response.Value as Item;
			Assert.IsNotNull(value, "Failed to retrieve item from the response. Value is null.");

			// Assert
			Assert.AreEqual(value, testItem);
		}

		[TestMethod]
		public async Task PutItem_Success_ReturnsTypeItem()
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

			_mockService.Setup(m => m.UpdateItem(It.IsAny<int>(), It.IsAny<Item>())).Returns(Task.FromResult(testItem));

			// Act => Assert
			var result = await _sut.PutItem(1, testItem);

			var response = result.Result as OkObjectResult;
			Assert.IsNotNull(response, "Response from PutItem is null. Expected OkObjectResult.");

			var value = response.Value as Item;
			Assert.IsNotNull(value, "Failed to retrieve item from the response. Value is null.");

			Assert.IsInstanceOfType(value, typeof(Item));
		}

		[TestMethod]
		public async Task PutItem_CallsServiceOnce()
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
			await _sut.PutItem(1, testItem);

			// Assert
			_mockService.Verify(m => m.UpdateItem(1, testItem));
		}

		[TestMethod]
		public async Task PutItem_ExceptionFromService_ThrowsSameException()
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
			_mockService.Setup(m => m.UpdateItem(It.IsAny<int>(), It.IsAny<Item>())).Throws(exception);

			// Act => Assert
			try
			{
				await _sut.PutItem(1, testItem);
				Assert.Fail("Error from Service should have come through");
			}
			catch (Exception e)
			{
				Assert.AreEqual(e.Message, exception.Message);
			}
		}

		[TestMethod]
		public async Task PostItem_CallsServiceOnce()
		{
			// Arrange
			var testItem = new Item()
			{
				Name = "TestItem",
				Status = 1,
				Category = 1
			};

			_mockService.Setup(m => m.AddItem(It.IsAny<Item>())).Returns(Task.FromResult(testItem));

			// Act
			await _sut.PostItem(testItem);

			// Assert
			_mockService.Verify(m => m.AddItem(testItem), Times.Once);
		}

		[TestMethod]
		public async Task PostItem_ExceptionFromService_ThrowsSameException()
		{
			// Arrange
			var testItem = new Item()
			{
				Name = "TestItem",
				Status = 1,
				Category = 1
			};

			var exception = new Exception("message");
			_mockService.Setup(m => m.AddItem(It.IsAny<Item>())).Throws(exception);

			// Act => Assert
			try
			{
				await _sut.PostItem(testItem);
				Assert.Fail("Error from Service should have come through");
			}
			catch (Exception e)
			{
				Assert.AreEqual(e.Message, exception.Message);
			}
		}

		[TestMethod]
		public async Task PostItem_Success_ReturnsCreatedAtAction()
		{
			// Arrange
			var testItem = new Item()
			{
				Name = "TestItem",
				Status = 1,
				Category = 1
			};

			_mockService.Setup(m => m.AddItem(It.IsAny<Item>())).Returns(Task.FromResult(testItem));

			// Act
			var result = await _sut.PostItem(testItem);

			// Assert
			Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
		}

		[TestMethod]
		public async Task PostItem_ReturnsSameContentAsTestData()
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

			_mockService.Setup(m => m.AddItem(testItem)).ReturnsAsync(testItem);

			// Act
			var result = await _sut.PostItem(testItem);

			var response = result.Result as CreatedAtActionResult;
			Assert.IsNotNull(response, "Response from PostItem is null. Expected CreatedAtActionResult.");

			var value = response.Value as Item;
			Assert.IsNotNull(value, "Failed to retrieve item from the response. Value is null.");

			// Assert
			Assert.AreEqual(value, testItem);
		}

		[TestMethod]
		public async Task PostItem_Success_ReturnsTypeItem()
		{
			// Arrange
			var testItem = new Item()
			{
				Name = "TestItem",
				Status = 1,
				Category = 1
			};

			_mockService.Setup(m => m.AddItem(It.IsAny<Item>())).Returns(Task.FromResult(testItem));

			// Act => Assert
			var result = await _sut.PostItem(testItem);

			var response = result.Result as CreatedAtActionResult;
			Assert.IsNotNull(response, "Response from PostItem is null. Expected CreatedAtActionResult.");

			var value = response.Value as Item;
			Assert.IsNotNull(value, "Failed to retrieve item from the response. Value is null.");

			Assert.IsInstanceOfType(value, typeof(Item));
		}

		[TestMethod]
		public async Task DeleteItem_Success_ReturnsNoContent()
		{
			// Arrange
			_mockService.Setup(m => m.DeleteItem(It.IsAny<int>())).ReturnsAsync(true);

			// Act => Assert
			var result = await _sut.DeleteItem(1);
			Assert.IsNotNull(result);

			Assert.IsInstanceOfType(result, typeof(NoContentResult));
		}

		[TestMethod]
		public async Task DeleteItem_CallsServiceOnce()
		{
			// Act
			var result = await _sut.DeleteItem(1);

			// Assert
			_mockService.Verify(m => m.DeleteItem(1), Times.Once);
		}

		[TestMethod]
		public async Task DeleteItem_ExceptionFromService_ThrowsSameException()
		{
			// Arrange
			var exception = new Exception("message");
			_mockService.Setup(m => m.DeleteItem(It.IsAny<int>())).Throws(exception);

			// Act => Assert
			try
			{
				await _sut.DeleteItem(1);
				Assert.Fail("Error from Service should have come through");
			}
			catch (Exception e)
			{
				Assert.AreEqual(e.Message, exception.Message);
			}
		}
	}
}