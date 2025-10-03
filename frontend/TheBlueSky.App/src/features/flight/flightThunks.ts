import { createAsyncThunk } from '@reduxjs/toolkit';
import { flightService } from '@/services/flights/flightService';
import type { Flight } from '@/types/flight';

export const fetchFlights = createAsyncThunk(
  'flights/fetchAll',
  async (_, { rejectWithValue }) => {
    try {
      const response = await flightService.getAll();
      return response;
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to fetch flights');
    }
  },
);

export const updateFlight = createAsyncThunk(
  'flights/update',
  async (flightData: Flight, { rejectWithValue }) => {
    try {
      await flightService.update(flightData.flightId, flightData);
      return flightData;
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to update flight');
    }
  },
);