export const daysOfWeek = [
  'Sunday',
  'Monday',
  'Tuesday',
  'Wednesday',
  'Thursday',
  'Friday',
  'Saturday',
] as const;

export type DayOfWeek = (typeof daysOfWeek)[number];

export type ScheduleDay = {
  scheduleDayId: number;
  flightScheduleId: number;
  dayOfWeek: number;
  isActive: boolean;
};