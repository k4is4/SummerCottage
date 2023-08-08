using Cottage.API.Exceptions;
using Cottage.API.Models;
using Cottage.API.Repositories;

namespace Cottage.API.Services
{
	public class CalendarEventsService: ICalendarEventsService
	{
		private readonly ICalendarEventsRepository _repository;

		public CalendarEventsService(ICalendarEventsRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public Task<List<CalendarEvent>> GetCalendarEvents()
		{
			return _repository.GetAll();
		}

		public async Task<CalendarEvent> GetCalendarEvent(int id)
		{
			if (id < 1)
			{
				throw new InvalidIdException();
			}

			var calendarEvent = await _repository.GetById(id);

			return calendarEvent ?? throw new ItemNotFoundException(id);
		}

		public async Task<CalendarEvent> UpdateCalendarEvent(int id, CalendarEvent calendarEvent)
		{
			if (id < 1)
			{
				throw new InvalidIdException();
			}

			if (id != calendarEvent.Id)
			{
				throw new ValidationsException("Mismatched item IDs");
			}

			calendarEvent.UpdatedOn = DateTime.UtcNow.AddHours(3);

			return await _repository.Update(calendarEvent) ?? throw new ItemNotFoundException(id);
		}

		public async Task<CalendarEvent> AddCalendarEvent(CalendarEvent calendarEvent)
		{
			calendarEvent.UpdatedOn = DateTime.UtcNow.AddHours(3);
			return await _repository.Add(calendarEvent);
		}

		public async Task<bool> DeleteCalendarEvent(int id)
		{
			if (id < 1)
			{
				throw new InvalidIdException();
			}

			var eventIsDeleted = await _repository.Delete(id);

			if (!eventIsDeleted)
			{
				throw new ItemNotFoundException(id);
			}

			return true;
		}
	}
}
