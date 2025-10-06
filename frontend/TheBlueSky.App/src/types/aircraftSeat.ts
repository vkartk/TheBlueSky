export const SeatClasses = [
  'Economy',
  'Business',
  'FirstClass'
] as const;

export type SeatClass = (typeof SeatClasses)[number];

export type AircraftSeat = {
  aircraftSeatId: number;
  aircraftId: number;
  seatClass: SeatClass;
  seatNumber: string;
  seatPosition: string;
  additionalFare: number;
  seatRow: number;
  seatColumn: number;
  isActive: boolean;
};

export type NewAircraftSeat = Omit<AircraftSeat, 'aircraftSeatId'>;