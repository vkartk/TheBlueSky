export const AIRCRAFT_MANUFACTURERS = [
  'Unknown',
  'Airbus',
  'Boeing'
] as const;

export type AircraftManufacturer = typeof AIRCRAFT_MANUFACTURERS[number];


export type Aircraft = {
  aircraftId: number;
  ownerUserId: string;
  aircraftName: string;
  aircraftModel: string;
  manufacturer: AircraftManufacturer;
  economySeats: number;
  businessSeats: number;
  firstClassSeats: number;
  isActive: boolean;
};

export type NewAircraft = Omit<Aircraft, 'aircraftId' | 'ownerUserId'>;