import type { AircraftSeat } from "./aircraftSeat";

export const SeatStatuses = [
  'Available',
  'Hold',
  'Reserved',
  'CheckedIn',
  'Boarded',
  'Blocked',
  'Inoperative',
] as const;

export type SeatStatus = (typeof SeatStatuses)[number];

export type FlightSeatStatus = {
    flightSeatStatusId: number;
    flightId: number;
    seatStatus: SeatStatus;
    aircraftSeat: AircraftSeat;
}