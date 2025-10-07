import { bookingsApiClient } from "./bookingsApiClient";
import type { Booking, CreateBookingRequest } from "@/types/booking";


export const createBooking = async (bookingData: CreateBookingRequest): Promise<Booking> => {
  try {
    const response = await bookingsApiClient.post<Booking>('/Booking', bookingData);
    return response.data;
  } catch (error) {
    console.error('Error creating booking:', error);
    throw new Error('Failed to create booking.');
  }
};
