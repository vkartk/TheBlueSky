import { bookingsApiClient } from "./bookingsApiClient";
import type { Booking, CreateBookingRequest } from "@/types/booking";

const API_PATH = '/Booking';

export const createBooking = async (bookingData: CreateBookingRequest): Promise<Booking> => {
  try {
    const response = await bookingsApiClient.post<Booking>(API_PATH, bookingData);
    return response.data;
  } catch (error) {
    console.error('Error creating booking:', error);
    throw new Error('Failed to create booking.');
  }
};

const getAll = async (): Promise<Booking[]> => {
  const res = await bookingsApiClient.get<Booking[]>(API_PATH);
  return res.data;
};

const getByUserId = async (userId: string): Promise<Booking[]> => {
  const res = await bookingsApiClient.get<Booking[]>(`${API_PATH}/user/${userId}`);
  return res.data;
};

const getById = async (bookingId: number): Promise<Booking> => {
    const res = await bookingsApiClient.get<Booking>(`${API_PATH}/${bookingId}`);
    return res.data;
};

const create = async (data: CreateBookingRequest): Promise<Booking> => {
  const res = await bookingsApiClient.post<Booking>(API_PATH, data);
  return res.data;
};

const update = async (id: number, data: Booking): Promise<Booking> => {
  const res = await bookingsApiClient.put<Booking>(`${API_PATH}`, data);
  return res.data;
};


export const bookingService = {
  getAll,
  getByUserId,
  getById,
  create,
  update,
};

