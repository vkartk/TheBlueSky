import type { AircraftSeat, NewAircraftSeat } from '@/types/aircraftSeat';
import { flightsApiClient } from './flightsApiClient';

const API_PATH = '/AircraftSeat';

const create = async (data: NewAircraftSeat): Promise<AircraftSeat> => {
  const response = await flightsApiClient.post<AircraftSeat>(API_PATH, data);
  return response.data;
};

const update = async (id: number, data: AircraftSeat): Promise<AircraftSeat> => {
  const response = await flightsApiClient.put<AircraftSeat>(`${API_PATH}/${id}`, data);
  return response.data;
};

export const aircraftSeatService = {
  create,
  update,
};