import type { Route, NewRoute } from '@/types/route';
import { flightsApiClient } from './flightsApiClient';

const API_PATH = '/routes';

const getAll = async (): Promise<Route[]> => {
  const response = await flightsApiClient.get<Route[]>(API_PATH);
  return response.data;
};

const create = async (data: NewRoute): Promise<Route> => {
  const response = await flightsApiClient.post<Route>(API_PATH, data);
  return response.data;
};

const update = async (id: number, data: Route): Promise<Route> => {
  const response = await flightsApiClient.put<Route>(`${API_PATH}/${id}`, data);
  return response.data;
};

export const routesService = {
  getAll,
  create,
  update,
};