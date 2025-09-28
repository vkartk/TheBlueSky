import axios from 'axios';
import type { Airport, NewAirport } from '@/types/airports';
import { ACCESS_KEY, FLIGHTS_BASE_URL } from '@/config';

const apiClient = axios.create({
  baseURL: FLIGHTS_BASE_URL,
});

apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem(ACCESS_KEY);
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

const getAll = async (): Promise<Airport[]> => {
  const response = await apiClient.get<Airport[]>('/airports');
  return response.data;
};

const create = async (data: NewAirport): Promise<Airport> => {
  const response = await apiClient.post<Airport>('/airports', data);
  return response.data;
};

const update = async (id: number, data: Airport): Promise<Airport> => {
  const response = await apiClient.put<Airport>(`/airports/${id}`, data);
  return response.data;
};

export const airportsService = {
  getAll,
  create,
  update,
};