using System.Collections.Generic;
using System.Threading.Tasks;
using Cottage.API.Models;

namespace Cottage.API.Services
{
	public interface IItemsService
	{
		Task<List<Item>> GetItems();
		Task<Item> GetItem(int id);
		Task<Item> UpdateItem(int id, Item item);
		Task<Item> AddItem(Item item);
		Task<bool> DeleteItem(int id);
	}
}
