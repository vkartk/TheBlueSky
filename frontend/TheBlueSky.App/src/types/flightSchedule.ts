export type FlightSchedule = {
  flightScheduleId: number;
  aircraftId: number;
  routeId: number;
  flightNumber: string;
  flightName: string | null;
  departureTime: string;
  arrivalTime: string;
  baseFare: number;
  checkinBaggageWeightKg: number;
  cabinBaggageWeightKg: number;
  validFrom: string;
  validUntil: string;
  isActive: boolean;
  createdDate: string;
};

export type NewFlightSchedule = Omit<FlightSchedule, 'flightScheduleId' | 'createdDate'>;