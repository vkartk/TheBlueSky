import { flightsApiClient } from './flightsApiClient';
import type { FlightSchedule, NewFlightSchedule } from '@/types/flightSchedule';

const API_URL = '/flightSchedule';

const getAll = async (): Promise<FlightSchedule[]> => {
  const response = await flightsApiClient.get<FlightSchedule[]>(API_URL);
  return response.data;
};

const create = async (data: NewFlightSchedule): Promise<FlightSchedule> => {
  const response = await flightsApiClient.post<FlightSchedule>(API_URL, data);
  return response.data;
};

const update = async (id: number, data: FlightSchedule): Promise<FlightSchedule> => {
  const response = await flightsApiClient.put<FlightSchedule>(`${API_URL}/${id}`, data);
  return response.data;
};

export const flightScheduleService = {
  getAll,
  create,
  update,
};