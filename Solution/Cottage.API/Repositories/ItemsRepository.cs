using Cottage.API.Exceptions;
using Cottage.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cottage.API.Repositories
{
	public class ItemsRepository : IItemsRepository
	{
		private readonly CottageContext _context;

		public ItemsRepository(CottageContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public Task<List<Item>> GetAll()
		{
			return _context.Items.ToListAsync();
		}

		public async Task<Item?> GetById(int id)
		{
			return await _context.Items.FindAsync(id);
		}

		public async Task<Item?> Update(Item item)
		{
			var dbItem = await _context.Items.FindAsync(item.Id);

			if (dbItem != null) 
			{ 			
			dbItem.Name = item.Name;
			dbItem.Status = item.Status;
			dbItem.Comment = item.Comment;
			dbItem.Category = item.Category;
			dbItem.UpdatedOn = item.UpdatedOn;

			await _context.SaveChangesAsync();
			}

			return dbItem;
		}

		public async Task<Item> Add(Item item)
		{
			_context.Items.Add(item);
			await _context.SaveChangesAsync();
			return item;
		}

		public async Task<bool> Delete(int id)
		{
			var item = await _context.Items.FindAsync(id);

			if (item == null)
				return false;

			_context.Items.Remove(item);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> DoesNameExistAsync(string itemName, int? excludedId = null)
		{
			return await _context.Items.AnyAsync(i => i.Name.ToLower() == itemName.ToLower() && (!excludedId.HasValue || i.Id != excludedId));
		}
	}
}
