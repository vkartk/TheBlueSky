import { createAsyncThunk } from '@reduxjs/toolkit';
import { seatClassService } from '@/services/flights/seatClassService';
import type { SeatClass, NewSeatClass } from '@/types/seatClass';

export const fetchSeatClasses = createAsyncThunk(
  'seatClasses/fetchAll',
  async (_, { rejectWithValue }) => {
    try {
      const response = await seatClassService.getAll();
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Failed to fetch seat classes');
    }
  }
);

export const createSeatClass = createAsyncThunk(
  'seatClasses/create',
  async (seatClassData: NewSeatClass, { rejectWithValue }) => {
    try {
      const response = await seatClassService.create(seatClassData);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Failed to create seat class');
    }
  }
);

export const updateSeatClass = createAsyncThunk(
  'seatClasses/update',
  async (seatClassData: SeatClass, { rejectWithValue }) => {
    try {
      await seatClassService.update(seatClassData.seatClassId, seatClassData);
      return seatClassData;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Failed to update seat class');
    }
  }
);