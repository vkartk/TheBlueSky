import { createAsyncThunk } from '@reduxjs/toolkit';
import { flightScheduleService } from '@/services/flights/flightScheduleService';
import type { FlightSchedule, NewFlightSchedule } from '@/types/flightSchedule';

export const fetchFlightSchedules = createAsyncThunk(
  'flightSchedules/fetchAll',
  async (_, { rejectWithValue }) => {
    try {
      return await flightScheduleService.getAll();
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to fetch flight schedules');
    }
  }
);

export const createFlightSchedule = createAsyncThunk(
  'flightSchedules/create',
  async (scheduleData: NewFlightSchedule, { rejectWithValue }) => {
    try {
      return await flightScheduleService.create(scheduleData);
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to create flight schedule');
    }
  }
);

export const updateFlightSchedule = createAsyncThunk(
  'flightSchedules/update',
  async (scheduleData: FlightSchedule, { rejectWithValue }) => {
    try {
      await flightScheduleService.update(scheduleData.flightScheduleId, scheduleData);
      return scheduleData;
    } catch (error: any) {
      return rejectWithValue(error.message || 'Failed to update flight schedule');
    }
  }
);