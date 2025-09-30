import { createAsyncThunk } from '@reduxjs/toolkit';
import { aircraftsService } from '@/services/flights/aircraftsService';
import type { Aircraft, NewAircraft } from '@/types/aircraft';

export const fetchAircrafts = createAsyncThunk(
  'aircrafts/fetchAll',
  async (_, { rejectWithValue }) => {
    try {
      return await aircraftsService.getAll();
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  }
);

export const createAircraft = createAsyncThunk(
  'aircrafts/create',
  async (aircraftData: NewAircraft, { rejectWithValue }) => {
    try {
      return await aircraftsService.create(aircraftData);
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  }
);

export const updateAircraft = createAsyncThunk(
  'aircrafts/update',
  async (aircraftData: Aircraft, { rejectWithValue }) => {
    try {
      await aircraftsService.update(aircraftData.aircraftId, aircraftData);
      return aircraftData;
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  }
);