import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { Booking } from '@/types/booking';
import {
  fetchBookings,
  fetchBookingsByUser,
  createBooking,
  updateBooking,
} from './bookingThunks';
import type { RootState } from '@/store';

interface BookingsState {
  items: Booking[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: BookingsState = {
  items: [],
  loading: 'idle',
  error: null,
};

const bookingsSlice = createSlice({
  name: 'bookings',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchBookings.pending, (state) => {
        state.loading = 'pending';
        state.error = null;
      })
      .addCase(fetchBookings.fulfilled, (state, action: PayloadAction<Booking[]>) => {
        state.loading = 'succeeded';
        state.items = action.payload;
      })
      .addCase(fetchBookings.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      .addCase(fetchBookingsByUser.pending, (state) => {
        state.loading = 'pending';
        state.error = null;
      })
      .addCase(fetchBookingsByUser.fulfilled, (state, action: PayloadAction<Booking[]>) => {
        state.loading = 'succeeded';
        state.items = action.payload;
      })
      .addCase(fetchBookingsByUser.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      // Create
      .addCase(createBooking.fulfilled, (state, action: PayloadAction<Booking>) => {
        state.items.push(action.payload);
      })

      // Update
      .addCase(updateBooking.fulfilled, (state, action: PayloadAction<Booking>) => {
        const index = state.items.findIndex((b) => b.bookingId === action.payload.bookingId);
        if (index !== -1) {
          state.items[index] = action.payload;
        }
      })
  },
});

export const selectAllBookings = (state: RootState) => state.bookings.items;
export const selectBookingsLoading = (state: RootState) => state.bookings.loading;
export const selectBookingsError = (state: RootState) => state.bookings.error;

export default bookingsSlice.reducer;