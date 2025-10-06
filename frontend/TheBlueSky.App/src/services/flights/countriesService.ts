import { flightsApiClient } from './flightsApiClient';
import type { Country } from '@/types/country';

const API_PATH = '/Countries';

const getAll = async (): Promise<Country[]> => {
  const res = await flightsApiClient.get<Country[]>(API_PATH);
  return res.data;
};

const getById = async (id: string): Promise<Country> => {
  const res = await flightsApiClient.get<Country>(`${API_PATH}/${id}`);
  return res.data;
};

export const countriesService = {
  getAll,
  getById,
};