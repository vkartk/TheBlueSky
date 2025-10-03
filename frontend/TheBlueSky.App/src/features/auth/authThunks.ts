import { createAsyncThunk } from '@reduxjs/toolkit';
import { toast } from 'sonner';

import { authService } from '@/services/auth/AuthService';
import type { LoginRequest, RegisterRequest, LoginResponse, User } from '@/types/auth';
import { logout } from './authSlice';

export const loginUser = createAsyncThunk<LoginResponse, LoginRequest>(
    'auth/login',
    async (credentials, { rejectWithValue }) => {
        try {
            const response = await authService.login(credentials);
            return response;
        } catch (error: any) {
            const errorMessage = error.response?.data?.message || 'Login failed. Please check your credentials.';
            return rejectWithValue(errorMessage);
        }
    }
);

export const registerUser = createAsyncThunk<LoginResponse, RegisterRequest>(
    'auth/register',
    async (userData, { dispatch, rejectWithValue }) => {
        try {
            await authService.register(userData);

            toast.success('Account created successfully! Logging you in...');
            const loginCredentials = { email: userData.email, password: userData.password };

            const loginAction = await dispatch(loginUser(loginCredentials));

            if (loginUser.rejected.match(loginAction)) {
                return rejectWithValue('Registration successful, but auto-login failed. Please log in manually.');
            }

            return loginAction.payload as LoginResponse;

        } catch (error: any) {
            const errorMessage = error.response?.data?.message || 'Registration failed. Please try again.';
            return rejectWithValue(errorMessage);
        }
    }
);

export const logoutUser = createAsyncThunk(
    'auth/logout',
    async (_, { dispatch, rejectWithValue }) => {
        try {
            await authService.logout();
            //  clear client state
            dispatch(logout());
        } catch (error: any) {
            const message = error.response?.data?.message || 'Logout failed on the server.';
            toast.error(message);
            // Even if server logout fails, we still log out on the client for better UX
            dispatch(logout());
            return rejectWithValue(message);
        }
    }
);

export const fetchCurrentUser = createAsyncThunk<User>(
    'auth/fetchCurrentUser',
    async (_, { rejectWithValue }) => {
        try {
            const user = await authService.fetchCurrentUser();
            return user;
        } catch (error: any) {
            return rejectWithValue('Failed to fetch user.');
        }
    }
);
