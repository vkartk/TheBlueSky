import type { Airport, NewAirport } from '@/types/airports';
import { flightsApiClient } from './flightsApiClient';

const getAll = async (): Promise<Airport[]> => {
  const response = await flightsApiClient.get<Airport[]>('/airports');
  return response.data;
};

const create = async (data: NewAirport): Promise<Airport> => {
  const response = await flightsApiClient.post<Airport>('/airports', data);
  return response.data;
};

const update = async (id: number, data: Airport): Promise<Airport> => {
  const response = await flightsApiClient.put<Airport>(`/airports/${id}`, data);
  return response.data;
};

export const airportsService = {
  getAll,
  create,
  update,
};