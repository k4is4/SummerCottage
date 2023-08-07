using System.Collections.Generic;
using System.Threading.Tasks;
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
			var item = await _repository.GetById(id);
			return item ?? throw new KeyNotFoundException("Id not found");
		}

		public async Task<Item> UpdateItem(int id, Item item)
		{
			if (item is null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			if (id != item.Id)
			{
				throw new ArgumentException("Mismatched item IDs");
			}

			item.UpdatedOn = DateTime.UtcNow;

			return await _repository.Update(item);	
		}

		public async Task<Item> AddItem(Item item)
		{
			item.UpdatedOn = DateTime.UtcNow;
			return await _repository.Add(item);
		}

		public async Task<bool> DeleteItem(int id)
		{
			 return await _repository.Delete(id);
		}
	}
}
