import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { User } from '@/types/auth';
import type { RootState } from '@/store';
import { fetchAllUsers, updateUser } from './usersThunks';

interface UsersState {
  users: User[];
  loading: 'idle' | 'pending' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: UsersState = {
  users: [],
  loading: 'idle',
  error: null,
};

const usersSlice = createSlice({
  name: 'users',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchAllUsers.pending, (state) => {
        state.loading = 'pending';
        state.error = null;
      })
      .addCase(fetchAllUsers.fulfilled, (state, action: PayloadAction<User[]>) => {
        state.loading = 'succeeded';
        state.users = action.payload;
      })
      .addCase(fetchAllUsers.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      })

      .addCase(updateUser.pending, (state) => {
        state.loading = 'pending';
        state.error = null;
      })
      .addCase(updateUser.fulfilled, (state, action: PayloadAction<User>) => {
        state.loading = 'succeeded';
        const updatedUser = action.payload;
        const index = state.users.findIndex(user => user.userId === updatedUser.userId);
        if (index !== -1) {
          state.users[index] = updatedUser;
        }
      })
      .addCase(updateUser.rejected, (state, action) => {
        state.loading = 'failed';
        state.error = action.payload as string;
      });
  },
});

export const selectAllUsers = (state: RootState) => state.users.users;

export default usersSlice.reducer;