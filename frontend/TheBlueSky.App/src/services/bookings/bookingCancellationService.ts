import { bookingsApiClient } from './bookingsApiClient';
import type { BookingCancellation, NewBookingCancellation } from '@/types/bookingCancellation';

const API_PATH = '/BookingCancellation';


const getAll = async (): Promise<BookingCancellation[]> => {
  const response = await bookingsApiClient.get<BookingCancellation[]>(API_PATH);
  return response.data;
};

const getByBookingId = async (bookingId: number): Promise<BookingCancellation> => {
    const response = await bookingsApiClient.get<BookingCancellation>(`${API_PATH}/by-booking/${bookingId}`);
    return response.data;
}


const create = async (data: NewBookingCancellation): Promise<BookingCancellation> => {
  const response = await bookingsApiClient.post<BookingCancellation>(API_PATH, data);
  return response.data;
};

export const bookingCancellationService = {
  getAll,
  getByBookingId,
  create,
};