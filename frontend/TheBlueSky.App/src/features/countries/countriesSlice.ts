import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { RootState } from '@/store';
import { fetchCountries } from './countriesThunks';
import type { Country } from '@/types/country';

interface CountriesState {
  items: Country[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: CountriesState = {
  items: [],
  loading: 'idle',
  error: null,
};

const countriesSlice = createSlice({
  name: 'countries',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchCountries.pending, (state) => {
        state.loading = 'pending';
        state.error = null;
      })
      .addCase(fetchCountries.fulfilled, (state, action: PayloadAction<Country[]>) => {
        state.loading = 'succeeded';
        state.items = action.payload;
      })
      .addCase(fetchCountries.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = (action.payload as string) ?? 'Failed to fetch countries';
      });
  },
});

export const selectAllCountries = (state: RootState) => state.countries.items;
export const selectCountriesLoading = (state: RootState) => state.countries.loading;
export const selectCountriesError = (state: RootState) => state.countries.error;

export default countriesSlice.reducer;
