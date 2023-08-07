using System.Collections.Generic;
using System.Threading.Tasks;
using Cottage.API.Exceptions;
using Cottage.API.Models;
using Cottage.API.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Cottage.API.Services
{
	public class ItemsService: IItemsService
	{
		private readonly IItemsRepository _repository;

		public ItemsService(IItemsRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public Task<List<Item>> GetItems()
		{
			return _repository.GetAll();
		}

		public async Task<Item> GetItem(int id)
		{
			if (id < 1)
			{
				throw new InvalidIdException();
			}

			var item = await _repository.GetById(id);

			return item ?? throw new ItemNotFoundException(id);
		}

		public async Task<Item> UpdateItem(int id, Item item)
		{
			if (id < 1)
			{
				throw new InvalidIdException();
			}

			if (id != item.Id)
			{
				throw new ValidationsException("Mismatched item IDs");
			}

			var items = await _repository.GetAll();
			var dbItem = items.FirstOrDefault(p => p.Id == id);

			if (dbItem is null)
			{
				throw new ItemNotFoundException(id);
			}

			var dbItemByName = items.FirstOrDefault(p => p.Name == item.Name);

			if (dbItem != dbItemByName)
			{
				if (items.Any(p => p.Name.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase)))
				{
					throw new ConflictException($"Item with the name '{item.Name}' exists already");
				}
			}

			item.UpdatedOn = DateTime.UtcNow;

			return await _repository.Update(item);	
		}

		public async Task<Item> AddItem(Item item)
		{
			var items = await _repository.GetAll();

			if (items.Any(p => p.Name.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase)))
			{
				throw new ConflictException($"Item with the name '{item.Name}' exists already");
			}

			item.UpdatedOn = DateTime.UtcNow;
			return await _repository.Add(item);
		}

		public async Task<bool> DeleteItem(int id)
		{
			if (id < 1)
			{
				throw new InvalidIdException();
			}

			var itemIsDeleted = await _repository.Delete(id);

			if (!itemIsDeleted)
			{
				throw new ItemNotFoundException(id);
			}

			return true;
		}
	}
}
