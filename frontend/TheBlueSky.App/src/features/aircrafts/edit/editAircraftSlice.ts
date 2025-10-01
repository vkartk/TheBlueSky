import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { Aircraft, AircraftWithSeats } from '@/types/aircraft';
import type { AircraftSeat } from '@/types/aircraftSeat';
import type { RootState } from '@/store';
import { updateAircraft } from '@/features/aircrafts/aircraftsThunks';
import {
  fetchAircraftWithSeats,
  createAircraftSeat,
  updateAircraftSeat,
} from './editAircraftThunks';

interface EditAircraftState {
  aircraft: Aircraft | null;
  seats: AircraftSeat[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: EditAircraftState = {
  aircraft: null,
  seats: [],
  loading: 'idle',
  error: null,
};

const editAircraftSlice = createSlice({
  name: 'editAircraft',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder

        // fetch aircraft with seats
      .addCase(fetchAircraftWithSeats.pending, (state) => {
        state.loading = 'pending';
        state.error = null;
      })
      .addCase(
        fetchAircraftWithSeats.fulfilled,
        (state, action: PayloadAction<AircraftWithSeats>) => {
          state.loading = 'succeeded';
          state.aircraft = action.payload.aircraft;
          state.seats = action.payload.seats;
        }
      )
      .addCase(fetchAircraftWithSeats.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      // create
      .addCase(createAircraftSeat.fulfilled, (state, action: PayloadAction<AircraftSeat>) => {
        state.seats.push(action.payload);
      })

      // update
      .addCase(updateAircraftSeat.fulfilled, (state, action: PayloadAction<AircraftSeat>) => {
        const index = state.seats.findIndex(
          (seat) => seat.aircraftSeatId === action.payload.aircraftSeatId
        );
        if (index !== -1) {
          state.seats[index] = action.payload;
        }
      })
  },
});

export const selectEditAircraft = (state: RootState) => state.editAircraft.aircraft;
export const selectEditAircraftSeats = (state: RootState) => state.editAircraft.seats;
export const selectEditAircraftLoading = (state: RootState) => state.editAircraft.loading;

export default editAircraftSlice.reducer;