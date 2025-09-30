import type { Aircraft, NewAircraft } from '@/types/aircraft';
import { flightsApiClient } from './flightsApiClient';

const API_PATH = '/aircrafts';

const getAll = async (): Promise<Aircraft[]> => {
  const response = await flightsApiClient.get<Aircraft[]>(API_PATH);
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

export const aircraftsService = {
  getAll,
  create,
  update,
};