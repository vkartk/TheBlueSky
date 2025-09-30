export type AircraftManufacturer = 'Unknown' | 'Airbus' | 'Boeing';

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

export type NewAircraft = Omit<Aircraft, 'aircraftId'>;