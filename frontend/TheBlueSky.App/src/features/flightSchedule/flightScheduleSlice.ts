import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { FlightSchedule } from '@/types/flightSchedule';
import { fetchFlightSchedules, createFlightSchedule, updateFlightSchedule } from './flightScheduleThunks';
import type { RootState } from '@/store';

interface FlightSchedulesState {
  items: FlightSchedule[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: FlightSchedulesState = {
  items: [],
  loading: 'idle',
  error: null,
};

const flightSchedulesSlice = createSlice({
  name: 'flightSchedules',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch All
      .addCase(fetchFlightSchedules.pending, (state) => {
        state.loading = 'pending';
      })
      .addCase(fetchFlightSchedules.fulfilled, (state, action: PayloadAction<FlightSchedule[]>) => {
        state.loading = 'succeeded';
        state.items = action.payload;
      })
      .addCase(fetchFlightSchedules.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      // Create
      .addCase(createFlightSchedule.fulfilled, (state, action: PayloadAction<FlightSchedule>) => {
        state.items.push(action.payload);
      })
      
      // Update
      .addCase(updateFlightSchedule.fulfilled, (state, action: PayloadAction<FlightSchedule>) => {
        const index = state.items.findIndex(
          (schedule) => schedule.flightScheduleId === action.payload.flightScheduleId
        );
        if (index !== -1) {
          state.items[index] = action.payload;
        }
      });
  },
});

export const selectAllFlightSchedules = (state: RootState) => state.flightSchedules.items;
export const selectFlightSchedulesLoading = (state: RootState) => state.flightSchedules.loading;

export default flightSchedulesSlice.reducer;