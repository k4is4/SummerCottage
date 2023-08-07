using Cottage.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cottage.API.Repositories
{
	public class CalendarEventsRepository : ICalendarEventsRepository
	{
		private readonly CottageContext _context;

		public CalendarEventsRepository(CottageContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public Task<List<CalendarEvent>> GetAll()
		{
			return _context.CalendarEvents.ToListAsync();
		}

		public async Task<CalendarEvent?> GetById(int id)
		{
			return await _context.CalendarEvents.FindAsync(id);
		}

		public async Task<CalendarEvent?> Update(CalendarEvent calendarEvent)
		{
			var dbEvent = await _context.CalendarEvents.FindAsync(calendarEvent.Id);

			if (dbEvent != null)
			{
				dbEvent.StartDate = calendarEvent.StartDate;
				dbEvent.EndDate = calendarEvent.EndDate;
				dbEvent.Note = calendarEvent.Note;
				dbEvent.Color = calendarEvent.Color;
				dbEvent.UpdatedOn = calendarEvent.UpdatedOn;

				await _context.SaveChangesAsync();
			}

			return dbEvent;
		}

		public async Task<CalendarEvent> Add(CalendarEvent calendarEvent)
		{
			_context.CalendarEvents.Add(calendarEvent);
			await _context.SaveChangesAsync();
			return calendarEvent;
		}

		public async Task<bool> Delete(int id)
		{
			var calendarEvent = await _context.CalendarEvents.FindAsync(id);
			if (calendarEvent == null)
				return false;
			_context.CalendarEvents.Remove(calendarEvent);
			return await _context.SaveChangesAsync() > 0;
		}
	}
}
