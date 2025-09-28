export type Airport = {
  airportId: number;
  airportCode: string;
  airportName: string;
  city: string;
  countryId: string;
  isActive: boolean;
};

export type NewAirport = Omit<Airport, 'airportId'>;