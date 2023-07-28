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
	public class CalendarNotesController : ControllerBase
	{
		private readonly CottageContext _context;

		public CalendarNotesController(CottageContext context)
		{
			_context = context;
		}

		// GET: api/CalendarNotes
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CalendarNote>>> GetCalendarNotes()
		{
			return await _context.CalendarNotes.ToListAsync();
		}

		// GET: api/CalendarNotes/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CalendarNote>> GetCalendarNote(int id)
		{
			var calendarNote = await _context.CalendarNotes.FindAsync(id);

			if (calendarNote == null)
			{
				return NotFound();
			}

			return calendarNote;
		}

		// PUT: api/CalendarNotes/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCalendarNote(int id, [FromBody] CalendarNote calendarNote)
		{
			if (id != calendarNote.Id)
			{
				return BadRequest();
			}

			_context.Entry(calendarNote).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CalendarNoteExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Ok(calendarNote);
		}

		// POST: api/CalendarNotes
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<CalendarNote>> PostCalendarNote([FromBody] CalendarNote calendarNote)
		{
			_context.CalendarNotes.Add(calendarNote);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCalendarNote", new { id = calendarNote.Id }, calendarNote);
		}

		// DELETE: api/CalendarNotes/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCalendarNote(int id)
		{
			var calendarNote = await _context.CalendarNotes.FindAsync(id);
			if (calendarNote == null)
			{
				return NotFound();
			}

			_context.CalendarNotes.Remove(calendarNote);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CalendarNoteExists(int id)
		{
			return _context.CalendarNotes.Any(e => e.Id == id);
		}
	}
}
