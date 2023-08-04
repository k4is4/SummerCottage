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

			if (items == null)
			{
				return NotFound();
			}

			return Ok(items);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Item>> GetItem(int id)
		{
			try
			{
				var item = await _service.GetItem(id);
				return Ok(item);
			}
			catch (NullReferenceException) 
			{ 
				return NotFound();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}

		}

		[HttpPut("{id}")]
		public async Task<ActionResult<Item>> PutItem(int id, [FromBody] Item item)
		{
			try
			{
				var updatedItem = await _service.UpdateItem(id, item);
				return Ok(updatedItem);
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpPost]
		public async Task<ActionResult<Item>> PostItem([FromBody] Item item)
		{
			try
			{
			var createdItem = await _service.AddItem(item);
			return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);

			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteItem(int id)
		{
			try
			{
				if (await _service.DeleteItem(id))
					return NoContent();
				else return NotFound();
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}
	}
}
