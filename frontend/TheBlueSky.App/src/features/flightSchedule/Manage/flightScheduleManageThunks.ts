import { createAsyncThunk } from '@reduxjs/toolkit';
import { flightScheduleManagementService } from '@/services/flights/flightScheduleManagementService';
import type { DayOfWeek } from '@/types/scheduleDay';

export const fetchScheduleDetails = createAsyncThunk(
  'flightScheduleManage/fetchDetails',
  async (scheduleId: number, { rejectWithValue }) => {
    try {
      return await flightScheduleManagementService.getScheduleDetails(scheduleId);
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || error.message);
    }
  }
);

export const fetchFlightsForSchedule = createAsyncThunk(
  'flightScheduleManage/fetchFlights',
  async (scheduleId: number, { rejectWithValue }) => {
    try {
      return await flightScheduleManagementService.getFlightsForSchedule(scheduleId);
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || error.message);
    }
  }
);

export const updateScheduleDays = createAsyncThunk(
  'flightScheduleManage/updateDays',
  async ({ scheduleId, days }: { scheduleId: number; days: DayOfWeek[] }, { rejectWithValue }) => {
    try {
      return await flightScheduleManagementService.updateScheduleDays(scheduleId, days);
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || error.message);
    }
  }
);

export const generateFlights = createAsyncThunk(
  'flightScheduleManage/generateFlights',
  async ({ scheduleId, startDate, endDate }: { scheduleId: number; startDate: string; endDate: string }, { dispatch, rejectWithValue }) => {
    try {
      const response = await flightScheduleManagementService.generateFlights(scheduleId, { startDate, endDate });
      // Re-fetch the flights list after successful generation
      dispatch(fetchFlightsForSchedule(scheduleId));
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || error.message);
    }
  }
);