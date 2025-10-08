import { createAsyncThunk } from '@reduxjs/toolkit';
import { toast } from 'sonner';
import { userService } from '@/services/auth/userService';
import type { User } from '@/types/auth';


export const fetchAllUsers = createAsyncThunk<User[]>(
  'users/fetchAll',
  async (_, { rejectWithValue }) => {
    try {
      const users = await userService.getAllUsers();
      return users;
    } catch (error: any) {
      const message = error.response?.data?.message || 'Failed to fetch users.';
      toast.error(message);
      return rejectWithValue(message);
    }
  }
);


export const updateUser = createAsyncThunk<User, User>(
  'users/update',
  async (userData, { rejectWithValue }) => {
    try {
      await userService.updateUser(userData);
      toast.success('User updated successfully!');
      return userData;
    } catch (error: any) {
      const message = error.response?.data?.message || 'Failed to update user.';
      toast.error(message);
      return rejectWithValue(message);
    }
  }
);