import type { ScheduleDay } from "./scheduleDay";

export type FlightStatus =
  | 'Scheduled'
  | 'Boarding'
  | 'Departed'
  | 'Arrived'
  | 'Delayed'
  | 'Cancelled'
  | 'Diverted';

export type GeneratedFlight = {
  flightId: number;
  flightScheduleId: number;
  flightDate: string; // "YYYY-MM-DD"
  departureDateTime: string;
  arrivalDateTime: string;
  flightStatus: FlightStatus;
  availableSeats: number;
};

export type FlightScheduleDetails = {
    flightScheduleId: number;
    flightNumber: string;
    aircraftId: number;
    routeId: number;
    departureTime: string; // "HH:mm:ss"
    arrivalTime: string; // "HH:mm:ss"
    scheduleDays: ScheduleDay[];
};