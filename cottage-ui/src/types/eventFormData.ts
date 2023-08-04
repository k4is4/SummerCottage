import { CalendarEventColor } from "./enums";

export default interface EventFormData {
	note: string;
	startDate: Date;
	endDate: Date;
	color: CalendarEventColor;
}
