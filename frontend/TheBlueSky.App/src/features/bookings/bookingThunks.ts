import { createAsyncThunk } from '@reduxjs/toolkit';
import { bookingService } from '@/services/bookings/bookingService';
import type { Booking, CreateBookingRequest } from '@/types/booking';

export const fetchBookings = createAsyncThunk(
  'bookings/fetchAll',
  async (_, { rejectWithValue }) => {
    try {
      return await bookingService.getAll();
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to fetch bookings');
    }
  }
);

export const fetchBookingsByUser = createAsyncThunk(
  'bookings/fetchByUser',
  async (userId: string, { rejectWithValue }) => {
    try {
      return await bookingService.getByUserId(userId);
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to fetch bookings for user');
    }
  }
);

export const createBooking = createAsyncThunk(
  'bookings/create',
  async (bookingData: CreateBookingRequest, { rejectWithValue }) => {
    try {
      return await bookingService.create(bookingData);
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to create booking');
    }
  }
);

export const updateBooking = createAsyncThunk(
  'bookings/update',
  async (bookingData: Booking, { rejectWithValue }) => {
    try {
      return await bookingService.update(bookingData.bookingId, bookingData);
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to update booking');
    }
  }
);