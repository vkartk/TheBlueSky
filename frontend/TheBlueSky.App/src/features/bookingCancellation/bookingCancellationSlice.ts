import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { RootState } from '@/store';
import type { BookingCancellation } from '@/types/bookingCancellation';
import { fetchAllCancellations, fetchCancellationByBookingId, createBookingCancellation } from './bookingCancellationThunks';


interface BookingCancellationsState {
  items: BookingCancellation[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: BookingCancellationsState = {
  items: [],
  loading: 'idle',
  error: null,
};

const bookingCancellationSlice = createSlice({
  name: 'bookingCancellations',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchAllCancellations.pending, (state) => {
        state.loading = 'pending';
        state.error = null;
      })
      .addCase(fetchAllCancellations.fulfilled, (state, action: PayloadAction<BookingCancellation[]>) => {
        state.loading = 'succeeded';
        state.items = action.payload;
      })
      .addCase(fetchAllCancellations.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      .addCase(fetchCancellationByBookingId.pending, (state) => {
        state.loading = 'pending';
        state.error = null;
      })
      .addCase(fetchCancellationByBookingId.fulfilled, (state, action: PayloadAction<BookingCancellation>) => {
        state.loading = 'succeeded';
        const index = state.items.findIndex(
          (c) => c.bookingCancellationId === action.payload.bookingCancellationId
        );
        if (index !== -1) {
          state.items[index] = action.payload;
        } else {
          state.items.push(action.payload);
        }
      })
       .addCase(fetchCancellationByBookingId.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      .addCase(createBookingCancellation.fulfilled, (state, action: PayloadAction<BookingCancellation>) => {
        state.items.push(action.payload);
      });
  },
});

export const selectAllCancellations = (state: RootState) => state.bookingCancellations.items;
export const selectCancellationsLoading = (state: RootState) => state.bookingCancellations.loading;
export const selectCancellationsError = (state: RootState) => state.bookingCancellations.error;

export default bookingCancellationSlice.reducer;