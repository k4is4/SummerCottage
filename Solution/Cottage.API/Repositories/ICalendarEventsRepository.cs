using Cottage.API.Models;

namespace Cottage.API.Repositories
{
	public interface ICalendarEventsRepository
	{
		Task<List<CalendarEvent>> GetAll();
		Task<CalendarEvent?> GetById(int id);
		Task<CalendarEvent> Add(CalendarEvent calendarEvent);
		Task<CalendarEvent?> Update(CalendarEvent calendarEvent);
		Task<bool> Delete(int id);
	}
}
