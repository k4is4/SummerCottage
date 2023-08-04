using System.Linq;
using System.Threading.Tasks;
using Cottage.API.Models;

namespace Cottage.API.Repositories
{
	public interface IItemsRepository
	{
		Task<List<Item>> GetAll();
		Task<Item?> GetById(int id);
		Task<Item> Update(Item item);
		Task<Item> Add(Item item);
		Task<bool> Delete(int id);
	}
}
