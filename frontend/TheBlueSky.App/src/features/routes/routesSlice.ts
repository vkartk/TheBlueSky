import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { Route } from '@/types/route';
import { fetchRoutes, createRoute, updateRoute } from './routesThunks';
import type { RootState } from '@/store';

interface RoutesState {
  items: Route[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: RoutesState = {
  items: [],
  loading: 'idle',
  error: null,
};

const routesSlice = createSlice({
  name: 'routes',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch Routes
      .addCase(fetchRoutes.pending, (state) => {
        state.loading = 'pending';
        state.error = null;
      })
      .addCase(fetchRoutes.fulfilled, (state, action: PayloadAction<Route[]>) => {
        state.loading = 'succeeded';
        state.items = action.payload;
      })
      .addCase(fetchRoutes.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      // Create Route
      .addCase(createRoute.fulfilled, (state, action: PayloadAction<Route>) => {
        state.items.push(action.payload);
      })

      // Update Route
      .addCase(updateRoute.fulfilled, (state, action: PayloadAction<Route>) => {
        const index = state.items.findIndex(
          (route) => route.routeId === action.payload.routeId
        );
        if (index !== -1) {
          state.items[index] = action.payload;
        }
      });
  },
});

export const selectAllRoutes = (state: RootState) => state.routes.items;
export const selectRoutesLoading = (state: RootState) => state.routes.loading;
export const selectRoutesError = (state: RootState) => state.routes.error;

export default routesSlice.reducer;