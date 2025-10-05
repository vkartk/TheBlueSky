import type { Aircraft } from "./aircraft";
import type { Airport } from "./airports";
import type { FlightStatus } from "./flight";

export type TripType = 'OneWay' | 'RoundTrip';

export interface FlightSearchRequest {
  routeId: number;
  departureDate: string; // YYYY-MM-DD
  returnDate?: string;   // YYYY-MM-DD
  tripType: TripType;
  adults: number;
}

export interface FlightSearchResponse {
  outboundFlights: FlightDetailResponse[];
  returnFlights?: FlightDetailResponse[];
}

export interface FlightDetailResponse {
  flightId: number;
  departureDateTime: string;
  arrivalDateTime: string;
  flightStatus: FlightStatus;
  availableSeats: number;
  baseFare: number;
  origin: Airport;
  destination: Airport;
  aircraft: Aircraft;
}
