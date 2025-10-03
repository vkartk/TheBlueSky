import { createSlice } from '@reduxjs/toolkit';
import { toast } from 'sonner';

import { loginUser, registerUser } from './authThunks';
import { ACCESS_KEY, REFRESH_KEY } from '@/config';
import type { User } from '@/types/auth';

const getAccessToken = () => localStorage.getItem(ACCESS_KEY);

interface AuthState {
    user: User | null;
    accessToken: string | null;
    isAuthenticated: boolean;
    loading: 'idle' | 'pending' | 'succeeded' | 'failed';
    error: string | null;
}

const initialState: AuthState = {
    user: null,
    accessToken: getAccessToken(),
    isAuthenticated: !!getAccessToken(),
    loading: 'idle',
    error: null,
};

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        logout: (state) => {
            state.user = null;
            state.accessToken = null;
            state.isAuthenticated = false;
            localStorage.removeItem(ACCESS_KEY);
            localStorage.removeItem(REFRESH_KEY);
            toast.info("You have been logged out.");
        },
    },
    extraReducers: (builder) => {
        builder
            // Login User
            .addCase(loginUser.pending, (state) => {
                state.loading = 'pending';
                state.error = null;
            })
            .addCase(loginUser.fulfilled, (state, action) => {

                state.loading = 'succeeded';
                state.isAuthenticated = true;
                state.accessToken = action.payload.accessToken ?? null;

                if (action.payload.accessToken && action.payload.refreshToken) {
                    localStorage.setItem(ACCESS_KEY, action.payload.accessToken);
                    localStorage.setItem(REFRESH_KEY, action.payload.refreshToken);
                }

                toast.success("Logged in successfully!");
            })
            .addCase(loginUser.rejected, (state, action) => {
                state.loading = 'failed';
                state.error = action.payload as string;
                toast.error(action.payload as string);
            })

            // Register User
            .addCase(registerUser.pending, (state) => {
                state.loading = 'pending';
                state.error = null;
            })
            .addCase(registerUser.fulfilled, (state, action) => {

                state.loading = 'succeeded';
                state.isAuthenticated = true;
                state.accessToken = action.payload.accessToken ?? null;

                if (action.payload.accessToken && action.payload.refreshToken) {
                    localStorage.setItem(ACCESS_KEY, action.payload.accessToken);
                    localStorage.setItem(REFRESH_KEY, action.payload.refreshToken);
                }
            })

            .addCase(registerUser.rejected, (state, action) => {
                state.loading = 'failed';
                state.error = action.payload as string;
                toast.error(action.payload as string);
            });
    },
});

export const { logout } = authSlice.actions;

export default authSlice.reducer;