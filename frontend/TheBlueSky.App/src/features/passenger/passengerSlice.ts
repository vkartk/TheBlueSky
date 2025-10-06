import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { Passenger } from '@/types/passenger';
import {
  fetchPassengers,
  fetchPassengerByUser,
  createPassenger,
  updatePassenger,
} from './passengerThunks';
import type { RootState } from '@/store';

interface PassengersState {
  items: Passenger[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: PassengersState = {
  items: [],
  loading: 'idle',
  error: null,
};

const passengersSlice = createSlice({
  name: 'passengers',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch All
      .addCase(fetchPassengers.pending, (state) => {
        state.loading = 'pending';
      })
      .addCase(fetchPassengers.fulfilled, (state, action: PayloadAction<Passenger[]>) => {
        state.loading = 'succeeded';
        state.items = action.payload;
      })
      .addCase(fetchPassengers.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      // Fetch by User
      .addCase(fetchPassengerByUser.pending, (state) => {
        state.loading = 'pending';
      })
      .addCase(fetchPassengerByUser.fulfilled, (state, action: PayloadAction<Passenger[]>) => {
        state.loading = 'succeeded';
        state.items = action.payload;
      })
      .addCase(fetchPassengerByUser.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      // Create
      .addCase(createPassenger.fulfilled, (state, action: PayloadAction<Passenger>) => {
        state.items.push(action.payload);
      })

      // Update
      .addCase(updatePassenger.fulfilled, (state, action: PayloadAction<Passenger>) => {
        const index = state.items.findIndex(
          (p) => p.passengerId === action.payload.passengerId
        );
        if (index !== -1) {
          state.items[index] = action.payload;
        }
      });
  },
});

export const selectAllPassengers = (state: RootState) => state.passengers.items;
export const selectPassengersLoading = (state: RootState) => state.passengers.loading;
export const selectPassengerError = (state: RootState) => state.passengers.error;

export default passengersSlice.reducer;