import { createAsyncThunk } from '@reduxjs/toolkit';
import type { Passenger, NewPassenger } from '@/types/passenger';
import { passengerService } from '@/services/bookings/passengerService';

export const fetchPassengers = createAsyncThunk(
  'passengers/fetchAll',
  async (_, { rejectWithValue }) => {
    try {
      return await passengerService.getAll();
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to fetch passengers');
    }
  }
);

export const fetchPassengerByUser = createAsyncThunk(
  'passengers/fetchByUser',
  async (userId: string, { rejectWithValue }) => {
    try {
      return await passengerService.getByUserId(userId);
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to fetch passenger');
    }
  }
);

export const createPassenger = createAsyncThunk(
  'passengers/create',
  async (passengerData: NewPassenger, { rejectWithValue }) => {
    try {
      return await passengerService.create(passengerData);
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to create passenger');
    }
  }
);

export const updatePassenger = createAsyncThunk(
  'passengers/update',
  async (passengerData: Passenger, { rejectWithValue }) => {
    try {
      await passengerService.update(passengerData.passengerId, passengerData);
      return passengerData;
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to update passenger');
    }
  }
);
