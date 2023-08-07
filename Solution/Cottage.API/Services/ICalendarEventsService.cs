using Cottage.API.Models;

namespace Cottage.API.Services
{
	public interface ICalendarEventsService
	{
		Task<List<CalendarEvent>> GetCalendarEvents();
		Task<CalendarEvent> GetCalendarEvent(int id);
		Task<CalendarEvent> AddCalendarEvent(CalendarEvent calendarEvent);
		Task<CalendarEvent> UpdateCalendarEvent(int id, CalendarEvent calendarEvent);
		Task<bool> DeleteCalendarEvent(int id);
	}
}
