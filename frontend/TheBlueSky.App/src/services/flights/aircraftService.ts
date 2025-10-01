import type { Aircraft, AircraftWithSeats, NewAircraft } from '@/types/aircraft';
import { flightsApiClient } from './flightsApiClient';

const API_PATH = '/aircraft';

const getAll = async (): Promise<Aircraft[]> => {
  const response = await flightsApiClient.get<Aircraft[]>(API_PATH);
  return response.data;
};

const getWithSeats = async (id: number): Promise<AircraftWithSeats> => {
  const response = await flightsApiClient.get<AircraftWithSeats>(`${API_PATH}/${id}/seats`);
  return response.data;
};

const create = async (data: NewAircraft): Promise<Aircraft> => {
  const response = await flightsApiClient.post<Aircraft>(API_PATH, data);
  return response.data;
};

const update = async (id: number, data: Aircraft): Promise<Aircraft> => {
  const response = await flightsApiClient.put<Aircraft>(`${API_PATH}/${id}`, data);
  return response.data;
};

export const aircraftService = {
  getAll,
  getWithSeats,
  create,
  update,
};