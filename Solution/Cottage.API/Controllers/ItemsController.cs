using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cottage.API.Models;
using Cottage.API.Services;

namespace Cottage.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		private readonly IItemsService _service;

		public ItemsController(IItemsService service)
		{
			_service = service ?? throw new ArgumentNullException(nameof(service));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Item>>> GetItems()
		{
			List<Item> items = await _service.GetItems();
			return Ok(items);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Item>> GetItem(int id)
		{
				var item = await _service.GetItem(id);
				return Ok(item);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<Item>> PutItem(int id, [FromBody] Item item)
		{
				var updatedItem = await _service.UpdateItem(id, item);
				return Ok(updatedItem);
		}

		[HttpPost]
		public async Task<ActionResult<Item>> PostItem([FromBody] Item item)
		{
			var createdItem = await _service.AddItem(item);
			return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteItem(int id)
		{
			await _service.DeleteItem(id);
			return NoContent();
		}
	}
}
