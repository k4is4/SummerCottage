using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cottage.API.Models;

namespace Cottage.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CalendarEventsController : ControllerBase
	{
		private readonly CottageContext _context;

		public CalendarEventsController(CottageContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetCalendarEvents()
		{
			return await _context.CalendarEvents.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CalendarEvent>> GetCalendarEvent(int id)
		{
			var calendarEvent = await _context.CalendarEvents.FindAsync(id);

			if (calendarEvent == null)
			{
				return NotFound();
			}

			return calendarEvent;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCalendarEvent(int id, [FromBody] CalendarEvent calendarEvent)
		{
			if (id != calendarEvent.Id)
			{
				return BadRequest();
			}

			calendarEvent.UpdatedOn = DateTime.Now;

			_context.Entry(calendarEvent).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CalendarEventExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Ok(calendarEvent);
		}

		[HttpPost]
		public async Task<ActionResult<CalendarEvent>> PostCalendarEvent([FromBody] CalendarEvent calendarEvent)
		{
			calendarEvent.UpdatedOn = DateTime.Now;
			_context.CalendarEvents.Add(calendarEvent);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCalendarEvent", new { id = calendarEvent.Id }, calendarEvent);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCalendarEvent(int id)
		{
			var calendarEvent = await _context.CalendarEvents.FindAsync(id);
			if (calendarEvent == null)
			{
				return NotFound();
			}

			_context.CalendarEvents.Remove(calendarEvent);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CalendarEventExists(int id)
		{
			return _context.CalendarEvents.Any(e => e.Id == id);
		}
	}
}
