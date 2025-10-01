import type { AircraftSeat } from "./aircraftSeat";

export const AIRCRAFT_MANUFACTURERS = ['Boeing', 'Airbus'] as const;

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

export type AircraftWithSeats = {
  aircraft: Aircraft;
  seats: AircraftSeat[];
};

export const AIRCRAFT_MODELS = [
  'Boeing 737',
  'Boeing 777',
  'Boeing 787 Dreamliner',
  'Airbus A320',
  'Airbus A350',
  'Airbus A380',
] as const;

export const MODELS_BY_MANUFACTURER: Record<AircraftManufacturer, string[]> = {
  Boeing: ['Boeing 737', 'Boeing 777', 'Boeing 787 Dreamliner'],
  Airbus: ['Airbus A320', 'Airbus A350', 'Airbus A380']
};

export const ALL_AIRCRAFT_MODELS = Object.values(MODELS_BY_MANUFACTURER).flat();
export type AircraftModel = (typeof ALL_AIRCRAFT_MODELS)[number];