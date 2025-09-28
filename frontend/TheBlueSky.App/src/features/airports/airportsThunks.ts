import { createAsyncThunk } from '@reduxjs/toolkit';
import { airportsService } from '@/services/airportsService';
import type { Airport, NewAirport } from '@/types/airports';

export const fetchAirports = createAsyncThunk(
  'airports/fetchAll',
  async (_, { rejectWithValue }) => {
    try {
      const response = await airportsService.getAll();
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Failed to fetch airports');
    }
  }
);

export const createAirport = createAsyncThunk(
  'airports/create',
  async (airportData: NewAirport, { rejectWithValue }) => {
    try {
      const response = await airportsService.create(airportData);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Failed to create airport');
    }
  }
);

export const updateAirport = createAsyncThunk(
  'airports/update',
  async (airportData: Airport, { rejectWithValue }) => {
    try {
      await airportsService.update(airportData.airportId, airportData);
      return airportData;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Failed to update airport');
    }
  }
);