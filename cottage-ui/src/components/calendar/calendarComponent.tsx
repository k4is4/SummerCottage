import React, { SyntheticEvent, useEffect, useState } from "react";
import { Calendar, momentLocalizer, SlotInfo } from "react-big-calendar";
import moment from "moment";
import "react-big-calendar/lib/css/react-big-calendar.css";
import { CalendarEventColor } from "../../types/enums";
import AddModal from "./addModal";
import EditModal from "./editModal";
import CalendarEvent from "../../types/calendarEvent";
import calendarEventService from "../../services/CalendarEventService";
import "moment/locale/fi";

const localizer = momentLocalizer(moment);

const CalendarComponent: React.FC = () => {
	const [events, setEvents] = useState<CalendarEvent[]>([]);
	const [selectedEvent, setSelectedEvent] = useState<CalendarEvent | null>(
		null
	);
	const [selectedSlot, setSelectedSlot] = useState<SlotInfo | null>(null);

	useEffect(() => {
		fetchEvents();
	}, []);

	const fetchEvents = async () => {
		try {
			const fetchedEvents: CalendarEvent[] =
				await calendarEventService.getCalendarEvents();
			setEvents(fetchedEvents);
		} catch (error) {
			console.error(error);
		}
	};

	const handleSlotSelection = (slotInfo: SlotInfo) => {
		setSelectedSlot(slotInfo);
	};

	const handleEventSelection = (event: CalendarEvent, e: SyntheticEvent) => {
		setSelectedEvent(event);
	};

	const handleEventUpdate = (
		oldEvent: CalendarEvent,
		updatedEvent: CalendarEvent
	) => {
		const updatedEventWithJsDates = {
			...updatedEvent,
			startDate: new Date(updatedEvent.startDate),
			endDate: new Date(updatedEvent.endDate),
		};
		setEvents(
			events.map((event) =>
				event.id === oldEvent.id ? updatedEventWithJsDates : event
			)
		);
		setSelectedEvent(null);
	};

	const handleEventDelete = (eventToDelete: CalendarEvent) => {
		setEvents(events.filter((event) => event.id !== eventToDelete.id));
		setSelectedEvent(null);
	};

	return (
		<div className="container">
			<Calendar
				views={["month"]}
				messages={{
					next: "Seuraava kuukausi >>",
					previous: "<< Edellinen kuukausi",
					today: "Tänään",
					month: "Kuukausi",
				}}
				localizer={localizer}
				events={events.map((event) => ({ ...event, title: event.note }))}
				startAccessor={(event) => event.startDate}
				endAccessor={(event) => event.endDate}
				style={{ height: 500, cursor: "pointer" }}
				eventPropGetter={(event: CalendarEvent) => ({
					style: {
						backgroundColor: CalendarEventColor[event.color],
						height: "30px",
					},
				})}
				selectable={true}
				onSelectSlot={handleSlotSelection}
				onSelectEvent={handleEventSelection}
			/>
			{selectedSlot && (
				<AddModal
					slotInfo={selectedSlot}
					onClose={() => setSelectedSlot(null)}
					onSave={(newEvent: CalendarEvent) => {
						setEvents((prevEvents) => [...prevEvents, newEvent]);
						setSelectedSlot(null);
					}}
				/>
			)}
			{selectedEvent && (
				<EditModal
					event={selectedEvent}
					onClose={() => setSelectedEvent(null)}
					onSave={handleEventUpdate}
					onDelete={handleEventDelete}
				/>
			)}
		</div>
	);
};

export default CalendarComponent;
