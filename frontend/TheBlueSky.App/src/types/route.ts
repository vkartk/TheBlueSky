export type Route = {
  routeId: number;
  originAirportId: number;
  destinationAirportId: number;
  distanceKm: number;
  estimatedDurationMinutes: number;
  isActive: boolean;
};

export type NewRoute = Omit<Route, 'routeId'>;