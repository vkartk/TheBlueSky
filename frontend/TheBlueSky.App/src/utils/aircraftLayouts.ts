import type { AircraftModel } from "@/types/aircraft";


export const AIRCRAFT_MODEL_CONFIGS: Record<AircraftModel, LayoutConfig> = {
  'Boeing 737': {
    name: 'Narrow-Body',
    layout: '3-3',
    seatSections: [3, 3],
    aisleCount: 1,
    totalSeatsPerRow: 6,
    columnLetters: ['A', 'B', 'C', 'D', 'E', 'F'],
  },
  'Airbus A320': {
    name: 'Narrow-Body',
    layout: '3-3', 
    seatSections: [3, 3], 
    aisleCount: 1, 
    totalSeatsPerRow: 6, 
    columnLetters: ['A', 'B', 'C', 'D', 'E', 'F'],
  },
  'Boeing 777': {
    name: 'Wide-Body', 
    layout: '3-4-3', 
    seatSections: [3, 4, 3], 
    aisleCount: 2, 
    totalSeatsPerRow: 10, 
    columnLetters: ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K'],
  },
  'Boeing 787 Dreamliner': {
    name: 'Wide-Body', 
    layout: '3-4-3', 
    seatSections: [3, 4, 3], 
    aisleCount: 2, 
    totalSeatsPerRow: 10, 
    columnLetters: ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K'],
  },
  'Airbus A350': {
    name: 'Wide-Body', 
    layout: '3-4-3', 
    seatSections: [3, 4, 3], 
    aisleCount: 2, 
    totalSeatsPerRow: 10, 
    columnLetters: ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K'],
  },
  'Airbus A380': {
    name: 'Super Wide-Body', 
    layout: '2-4-2', 
    seatSections: [2, 4, 2], 
    aisleCount: 2, 
    totalSeatsPerRow: 8, 
    columnLetters: ['A', 'B', 'D', 'E', 'F', 'G', 'J', 'K'],
  }
}
export type LayoutConfig = {
  name: string;
  layout: string;
  seatSections: number[];
  aisleCount: number;
  totalSeatsPerRow: number;
  columnLetters: string[];
};

export const getLayoutFromModel = (model: AircraftModel | string): LayoutConfig => {
  return AIRCRAFT_MODEL_CONFIGS[model as AircraftModel];
};
