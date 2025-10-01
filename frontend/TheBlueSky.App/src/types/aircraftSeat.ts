export type AircraftSeat = {
  aircraftSeatId: number;
  aircraftId: number;
  seatClassId: number;
  seatNumber: string;
  seatPosition: string;
  additionalFare: number;
  seatRow: number;
  seatColumn: number;
  isActive: boolean;
};

export type NewAircraftSeat = Omit<AircraftSeat, 'aircraftSeatId'>;