import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { RootState } from '@/store';
import type { ScheduleDay } from '@/types/scheduleDay';
import type { GeneratedFlight, FlightScheduleDetails } from '@/types/generatedFlight';
import {
  fetchScheduleDetails,
  fetchFlightsForSchedule,
  updateScheduleDays,
  generateFlights,
} from './flightScheduleManageThunks';

interface FlightScheduleManageState {
  details: FlightScheduleDetails | null;
  scheduleDays: ScheduleDay[];
  generatedFlights: GeneratedFlight[];
  loading: {
    details: boolean;
    flights: boolean;
    isSavingDays: boolean;
    isGenerating: boolean;
  };
  error: string | null;
}

const initialState: FlightScheduleManageState = {
  details: null,
  scheduleDays: [],
  generatedFlights: [],
  loading: {
    details: false,
    flights: false,
    isSavingDays: false,
    isGenerating: false,
  },
  error: null,
};

const flightScheduleManageSlice = createSlice({
  name: 'flightScheduleManage',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch Details
      .addCase(fetchScheduleDetails.pending, (state) => {
        state.loading.details = true;
      })
      .addCase(fetchScheduleDetails.fulfilled, (state, action: PayloadAction<FlightScheduleDetails>) => {
        state.loading.details = false;
        state.details = action.payload;
        state.scheduleDays = action.payload.scheduleDays || [];
      })
      .addCase(fetchScheduleDetails.rejected, (state, action) => {
        state.loading.details = false;
        state.error = action.payload as string;
      })

      // Fetch Generated Flights
      .addCase(fetchFlightsForSchedule.pending, (state) => {
        state.loading.flights = true;
      })
      .addCase(fetchFlightsForSchedule.fulfilled, (state, action: PayloadAction<GeneratedFlight[]>) => {
        state.loading.flights = false;
        state.generatedFlights = action.payload;
      })
      .addCase(fetchFlightsForSchedule.rejected, (state, action) => {
        state.loading.flights = false;
        state.error = action.payload as string;
      })

      // Update Schedule Days
      .addCase(updateScheduleDays.pending, (state) => {
        state.loading.isSavingDays = true;
      })
      .addCase(updateScheduleDays.fulfilled, (state, action: PayloadAction<ScheduleDay[]>) => {
        state.loading.isSavingDays = false;
        state.scheduleDays = action.payload;
      })
      .addCase(updateScheduleDays.rejected, (state, action) => {
        state.loading.isSavingDays = false;
        state.error = action.payload as string;
      })
      
      // Generate Flights
      .addCase(generateFlights.pending, (state) => {
        state.loading.isGenerating = true;
      })
      .addCase(generateFlights.fulfilled, (state) => {
        state.loading.isGenerating = false;
      })
      .addCase(generateFlights.rejected, (state, action) => {
        state.loading.isGenerating = false;
        state.error = action.payload as string;
      });
  },
});

// Selectors
export const selectScheduleDetails = (state: RootState) => state.flightScheduleManage.details;
export const selectScheduleDays = (state: RootState) => state.flightScheduleManage.scheduleDays;
export const selectGeneratedFlights = (state: RootState) => state.flightScheduleManage.generatedFlights;
export const selectScheduleManageLoading = (state: RootState) => state.flightScheduleManage.loading;
export const selectScheduleManageError = (state: RootState) => state.flightScheduleManage.error;

export default flightScheduleManageSlice.reducer;