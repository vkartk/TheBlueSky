import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { Airport } from '@/types/airports';
import { fetchAirports, createAirport, updateAirport } from './airportsThunks';
import type { RootState } from '@/store';

interface AirportsState {
  items: Airport[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: AirportsState = {
  items: [],
  loading: 'idle',
  error: null,
};

const airportsSlice = createSlice({
  name: 'airports',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder

      // Fetch Airports
      .addCase(fetchAirports.pending, (state) => {
        state.loading = 'pending';
        state.error = null;
      })
      .addCase(fetchAirports.fulfilled, (state, action: PayloadAction<Airport[]>) => {
        state.loading = 'succeeded';
        state.items = action.payload;
      })
      .addCase(fetchAirports.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      // Create Airport
      .addCase(createAirport.fulfilled, (state, action: PayloadAction<Airport>) => {
        state.items.push(action.payload);
      })


      // Update Airport
      .addCase(updateAirport.fulfilled, (state, action: PayloadAction<Airport>) => {
        const index = state.items.findIndex(
          (airport) => airport.airportId === action.payload.airportId
        );

        if (index !== -1) {
          state.items[index] = action.payload;
        }
      });
  },
});

export const selectAllAirports = (state: RootState) => state.airports.items;
export const selectAirportsLoading = (state: RootState) => state.airports.loading;
export const selectAirportsError = (state: RootState) => state.airports.error;

export default airportsSlice.reducer;