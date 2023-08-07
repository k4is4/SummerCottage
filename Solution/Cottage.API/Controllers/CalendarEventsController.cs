using Microsoft.AspNetCore.Mvc;
using Cottage.API.Models;
using Cottage.API.Services;

namespace Cottage.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CalendarEventsController : ControllerBase
	{
		private readonly ICalendarEventsService _service;

		public CalendarEventsController(ICalendarEventsService service)
		{
			_service = service ?? throw new ArgumentNullException(nameof(service));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetCalendarEvents()
		{
			List<CalendarEvent> events = await _service.GetCalendarEvents();
			return Ok(events);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CalendarEvent>> GetCalendarEvent(int id)
		{
			var calendarEvent = await _service.GetCalendarEvent(id);
			return Ok(calendarEvent);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCalendarEvent(int id, [FromBody] CalendarEvent calendarEvent)
		{
			var updatedEvent = await _service.UpdateCalendarEvent(id, calendarEvent);
			return Ok(updatedEvent);
		}

		[HttpPost]
		public async Task<ActionResult<CalendarEvent>> PostCalendarEvent([FromBody] CalendarEvent calendarEvent)
		{
			var createdEvent = await _service.AddCalendarEvent(calendarEvent);
			return CreatedAtAction(nameof(GetCalendarEvent), new { id = createdEvent.Id }, createdEvent);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCalendarEvent(int id)
		{
			await _service.DeleteCalendarEvent(id);
			return NoContent();
		}
	}
}
