import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { SeatClass } from '@/types/seatClass';
import { fetchSeatClasses, createSeatClass, updateSeatClass } from './seatClassThunks';
import type { RootState } from '@/store'; 

interface SeatClassesState {
  items: SeatClass[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: SeatClassesState = {
  items: [],
  loading: 'idle',
  error: null,
};

const seatClassesSlice = createSlice({
  name: 'seatClasses',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder

      // Fetch
      .addCase(fetchSeatClasses.pending, (state) => {
        state.loading = 'pending';
      })
      .addCase(fetchSeatClasses.fulfilled, (state, action: PayloadAction<SeatClass[]>) => {
        state.loading = 'succeeded';
        state.items = action.payload;
      })
      .addCase(fetchSeatClasses.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      // Create
      .addCase(createSeatClass.fulfilled, (state, action: PayloadAction<SeatClass>) => {
        state.items.push(action.payload);
      })
      
      // Update
      .addCase(updateSeatClass.fulfilled, (state, action: PayloadAction<SeatClass>) => {
        const index = state.items.findIndex(
          (sc) => sc.seatClassId === action.payload.seatClassId
        );
        if (index !== -1) {
          state.items[index] = action.payload;
        }
      });
  },
});

export const selectAllSeatClasses = (state: RootState) => state.seatClasses.items;
export const selectSeatClassesLoading = (state: RootState) => state.seatClasses.loading;

export default seatClassesSlice.reducer;