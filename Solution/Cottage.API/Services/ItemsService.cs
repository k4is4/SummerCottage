using Cottage.API.Exceptions;
using Cottage.API.Models;
using Cottage.API.Repositories;

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

			bool nameExists = await _repository.DoesNameExistAsync(item.Name, id);
			if (nameExists)
			{
				throw new ConflictException($"Item with the name '{item.Name}' exists already");
			}

			item.UpdatedOn = DateTime.UtcNow.AddHours(3);

			return await _repository.Update(item) ?? throw new ItemNotFoundException(id);	
		}

		public async Task<Item> AddItem(Item item)
		{
			bool nameExists = await _repository.DoesNameExistAsync(item.Name);
			if (nameExists)
			{
				throw new ConflictException($"Item with the name '{item.Name}' exists already");
			}

			item.UpdatedOn = DateTime.UtcNow.AddHours(3);
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
