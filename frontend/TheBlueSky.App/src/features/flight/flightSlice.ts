import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { Flight } from '@/types/flight';
import { fetchFlights, updateFlight } from './flightThunks';
import type { RootState } from '@/store';

interface FlightsState {
  items: Flight[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: FlightsState = {
  items: [],
  loading: 'idle',
  error: null,
};

const flightsSlice = createSlice({
  name: 'flights',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch Flights
      .addCase(fetchFlights.pending, (state) => {
        state.loading = 'pending';
      })
      .addCase(
        fetchFlights.fulfilled,
        (state, action: PayloadAction<Flight[]>) => {
          state.loading = 'succeeded';
          state.items = action.payload;
        },
      )
      .addCase(fetchFlights.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      // Update Flight
      .addCase(updateFlight.pending, (state) => {
        state.loading = 'pending';
      })
      .addCase(
        updateFlight.fulfilled,
        (state, action: PayloadAction<Flight>) => {
          state.loading = 'succeeded';
          const index = state.items.findIndex(
            (flight) => flight.flightId === action.payload.flightId,
          );
          if (index !== -1) {
            state.items[index] = action.payload;
          }
        },
      )
      .addCase(updateFlight.rejected, (state, action) => {
          state.loading = 'failed';
          state.error = action.payload as string;
      });
  },
});

export const selectAllFlights = (state: RootState) => state.flights.items;
export const selectFlightsLoading = (state: RootState) => state.flights.loading;

export default flightsSlice.reducer;