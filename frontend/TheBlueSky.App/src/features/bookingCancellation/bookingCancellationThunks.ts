import { createAsyncThunk } from '@reduxjs/toolkit';
import { bookingCancellationService } from '@/services/bookings/bookingCancellationService';
import type { NewBookingCancellation } from '@/types/bookingCancellation';

export const fetchAllCancellations = createAsyncThunk(
  'bookingCancellations/fetchAll',
  async (_, { rejectWithValue }) => {
    try {
      return await bookingCancellationService.getAll();
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to fetch booking cancellations');
    }
  }
);

export const fetchCancellationByBookingId = createAsyncThunk(
  'bookingCancellations/fetchByBookingId',
  async (bookingId: number, { rejectWithValue }) => {
    try {
        return await bookingCancellationService.getByBookingId(bookingId);
    } catch (error: any) {
        return rejectWithValue(error.message || 'Failed to fetch cancellation for booking');
    }
  }
)

export const createBookingCancellation = createAsyncThunk(
  'bookingCancellations/create',
  async (cancellationData: NewBookingCancellation, { rejectWithValue }) => {
    try {
      return await bookingCancellationService.create(cancellationData);
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to create booking cancellation record');
    }
  }
);