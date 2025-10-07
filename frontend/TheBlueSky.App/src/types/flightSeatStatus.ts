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

export type flightSeatStatus = {
    flightSeatStatusId: number;
    flightId: number;
    seatStatus: SeatStatus
}