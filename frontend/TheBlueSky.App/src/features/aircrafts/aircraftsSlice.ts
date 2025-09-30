import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { Aircraft } from '@/types/aircraft';
import { fetchAircrafts, createAircraft, updateAircraft } from './aircraftsThunks';
import type { RootState } from '@/store';

interface AircraftsState {
  items: Aircraft[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: AircraftsState = {
  items: [],
  loading: 'idle',
  error: null,
};

const aircraftsSlice = createSlice({
  name: 'aircrafts',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder

      // Fetch Aircrafts
      .addCase(fetchAircrafts.pending, (state) => {
        state.loading = 'pending';
      })
      .addCase(fetchAircrafts.fulfilled, (state, action: PayloadAction<Aircraft[]>) => {
        state.loading = 'succeeded';
        state.items = action.payload;
      })
      .addCase(fetchAircrafts.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      // Create Aircraft
      .addCase(createAircraft.fulfilled, (state, action: PayloadAction<Aircraft>) => {
        state.items.push(action.payload);
      })
      
      // Update Aircraft
      .addCase(updateAircraft.fulfilled, (state, action: PayloadAction<Aircraft>) => {
        const index = state.items.findIndex(
          (aircraft) => aircraft.aircraftId === action.payload.aircraftId
        );
        if (index !== -1) {
          state.items[index] = action.payload;
        }
      });
  },
});

export const selectAllAircrafts = (state: RootState) => state.aircrafts.items;
export const selectAircraftsLoading = (state: RootState) => state.aircrafts.loading;

export default aircraftsSlice.reducer;