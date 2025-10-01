import { createAsyncThunk } from '@reduxjs/toolkit';
import { aircraftService } from '@/services/flights/aircraftService';
import { aircraftSeatService } from '@/services/flights/aircraftSeatService';
import type { AircraftSeat, NewAircraftSeat } from '@/types/aircraftSeat';

export const fetchAircraftWithSeats = createAsyncThunk(
    'editAircraft/fetchWithSeats',
    async (aircraftId: number, { rejectWithValue }) => {
        try {
            return await aircraftService.getWithSeats(aircraftId);
        } catch (error: any) {
            return rejectWithValue(error.response?.data?.message ?? error.message);
        }
    }
);

export const createAircraftSeat = createAsyncThunk(
    'editAircraft/createSeat',
    async (seatData: NewAircraftSeat, { rejectWithValue }) => {
        try {
            return await aircraftSeatService.create(seatData);
        } catch (error: any) {
            return rejectWithValue(error.response?.data?.message ?? error.message);
        }
    }
);

export const updateAircraftSeat = createAsyncThunk(
    'editAircraft/updateSeat',
    async (seatData: AircraftSeat, { rejectWithValue }) => {
        try {
            await aircraftSeatService.update(seatData.aircraftSeatId, seatData);
            return seatData;
        } catch (error: any) {
            return rejectWithValue(error.response?.data?.message ?? error.message);
        }
    }
);