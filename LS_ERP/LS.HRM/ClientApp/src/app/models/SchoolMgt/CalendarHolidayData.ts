export interface CalendarHolidayDataDto {
  branchCode: string;
  startDate: any;
  endDate: any;
  eventsHolidaysDataList: Array<EventsHolidaysDataDto>;
}

export interface EventsHolidaysDataDto {
  eventDate: any;
  eventName: string;
  eventType: number;
}
