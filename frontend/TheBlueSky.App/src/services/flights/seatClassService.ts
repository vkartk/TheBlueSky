import type { SeatClass, NewSeatClass } from '@/types/seatClass';
import { flightsApiClient } from './flightsApiClient';

const API_PATH = '/seatclasses';

const getAll = async (): Promise<SeatClass[]> => {
  const response = await flightsApiClient.get<SeatClass[]>(API_PATH);
  return response.data;
};

const create = async (data: NewSeatClass): Promise<SeatClass> => {
  const response = await flightsApiClient.post<SeatClass>(API_PATH, data);
  return response.data;
};

const update = async (id: number, data: SeatClass): Promise<SeatClass> => {
  const response = await flightsApiClient.put<SeatClass>(`${API_PATH}/${id}`, data);
  return response.data;
};

export const seatClassService = {
  getAll,
  create,
  update,
};