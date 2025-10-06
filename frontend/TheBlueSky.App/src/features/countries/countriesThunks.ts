import { createAsyncThunk } from '@reduxjs/toolkit';

import { countriesService } from '@/services/flights/countriesService';
import type { Country } from '@/types/country';

export const fetchCountries = createAsyncThunk(
  'countries/fetchAll',
  async (_, { rejectWithValue }) => {
    try {
      const response: Country[] = await countriesService.getAll();
      return response;
    } catch (error: any) {
      return rejectWithValue(error?.message || 'Failed to fetch countries');
    }
  },
);