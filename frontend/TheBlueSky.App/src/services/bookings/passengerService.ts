import { bookingsApiClient } from './bookingsApiClient';
import type { Passenger, NewPassenger } from '@/types/passenger';

const API_PATH = '/Passenger';

const getAll = async (): Promise<Passenger[]> => {
  const res = await bookingsApiClient.get<Passenger[]>(API_PATH);
  return res.data;
};

const getByUserId = async (userId: string): Promise<Passenger[]> => {
  const res = await bookingsApiClient.get<Passenger[]>(`${API_PATH}/managed-by/${userId}`);
  return res.data;
};

const create = async (data: NewPassenger): Promise<Passenger> => {
  const res = await bookingsApiClient.post<Passenger>(API_PATH, data);
  return res.data;
};

const update = async (id: number, data: Passenger): Promise<Passenger> => {
  const res = await bookingsApiClient.put<Passenger>(`${API_PATH}`, data);
  return res.data;
};

export const passengerService = {
  getAll,
  getByUserId,
  create,
  update
};
