import { createAsyncThunk } from '@reduxjs/toolkit';
import { routesService } from '@/services/flights/routesService';
import type { Route, NewRoute } from '@/types/route';


export const fetchRoutes = createAsyncThunk(
  'routes/fetchAll',
  async (_, { rejectWithValue }) => {
    try {
      const response = await routesService.getAll();
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Failed to fetch routes');
    }
  }
);


export const createRoute = createAsyncThunk(
  'routes/create',
  async (routeData: NewRoute, { rejectWithValue }) => {
    try {
      const response = await routesService.create(routeData);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Failed to create route');
    }
  }
);

export const updateRoute = createAsyncThunk(
  'routes/update',
  async (routeData: Route, { rejectWithValue }) => {
    try {
      await routesService.update(routeData.routeId, routeData);
      return routeData;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Failed to update route');
    }
  }
);