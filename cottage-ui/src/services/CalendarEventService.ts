import { AxiosError, AxiosResponse } from "axios";
import CalendarEvent from "../types/calendarEvent";
import apiClient from "./apiClient";
import CalendarEventWithoutId from "../types/calendarEventWithoutId";
import { ProblemDetails } from "../types/problemDetails";

class CalendarEventService {
	async getCalendarEvents(): Promise<CalendarEvent[]> {
		return apiClient
			.get<CalendarEvent[]>("/CalendarNotes")
			.then((response: AxiosResponse<CalendarEvent[]>) => response.data);
	}

	async addCalendarEvent(
		event: CalendarEventWithoutId
	): Promise<CalendarEvent> {
		return apiClient
			.post<CalendarEvent>("/CalendarNotes", event)
			.then((response: AxiosResponse<CalendarEvent>) => response.data)
			.catch(this.parseError<CalendarEvent>);
	}

	async updateCalendarEvent(event: CalendarEvent): Promise<CalendarEvent> {
		return apiClient
			.put<CalendarEvent>(`/CalendarNotes/${event.id}`, event)
			.then((response: AxiosResponse<CalendarEvent>) => response.data)
			.catch(this.parseError<CalendarEvent>);
	}

	async deleteCalendarEvent(id: number): Promise<void> {
		return apiClient.delete(`/CalendarNotes/${id}`);
	}

	parseError<T>(errorResponse: AxiosError<ProblemDetails>): Promise<T> {
		const errors = errorResponse.response?.data.errors;
		if (errors) {
			let errorsToDisplay: string = "";
			Object.keys(errors).forEach((key: string) => {
				errorsToDisplay += errors[key] + "\n";
			});
			throw Error(errorsToDisplay);
		} else {
			throw Error(errorResponse.response?.data);
		}
	}
}

const calendarEventService = new CalendarEventService();
export default calendarEventService;
