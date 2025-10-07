import type { Aircraft } from "./aircraft";
import type { AircraftSeat } from "./aircraftSeat";
import type { FlightSchedule } from "./flightSchedule";
import type { flightSeatStatus } from "./flightSeatStatus";

export const flightStatuses = [
  'Scheduled',
  'Boarding',
  'Departed',
  'Arrived',
  'Delayed',
  'Cancelled',
  'Diverted',
] as const;

export type FlightStatus = (typeof flightStatuses)[number];

export type Flight = {
  flightId: number;
  flightScheduleId: number;
  flightDate: string; // YYYY-MM-DD
  departureDateTime: string;
  arrivalDateTime: string;
  flightStatus: FlightStatus;
  baseFare: number,
  availableSeats: number;
};

export type GetFlight = Flight &{
  schedule: FlightSchedule & {
    aircraft: Aircraft
  },
  seatStatuses: flightSeatStatus & {
    aircraftSeat: AircraftSeat
  }
}