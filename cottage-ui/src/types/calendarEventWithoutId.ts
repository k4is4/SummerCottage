export default interface CalendarEventWithoutId {
	note: string;
	startDate: Date;
	endDate: Date;
	color: number;
	updatedOn: string | undefined;
}
