import axios, { AxiosError, type InternalAxiosRequestConfig } from 'axios';
import { store } from '@/store';
import { API_BASE_URL, REFRESH_KEY } from '@/config';
import { logout, tokensRefreshed } from '@/features/auth/authSlice';

export const attachAuthToken = (config: InternalAxiosRequestConfig) => {

    const { accessToken } = store.getState().auth;

    if (accessToken) config.headers.Authorization = `Bearer ${accessToken}`;
    return config;
};

export const handleTokenRefresh = async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };

    if (error.response?.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;
        const refreshToken = localStorage.getItem(REFRESH_KEY);

        if (refreshToken) {
            try {
                const { accessToken } = store.getState().auth;

                const response = await axios.post(`${API_BASE_URL}/auth/refresh-token`, {
                    accessToken: accessToken,
                    refreshToken: refreshToken,
                });

                const { accessToken: newAccessToken, refreshToken: newRefreshToken } = response.data;
                
                store.dispatch(tokensRefreshed({ accessToken: newAccessToken, refreshToken: newRefreshToken }));

                if (originalRequest.headers) {
                    originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
                }

                return axios(originalRequest);
            } catch (refreshError) {
                store.dispatch(logout());
                return Promise.reject(refreshError);
            }
        } else {
            store.dispatch(logout());
        }
    }

    return Promise.reject(error);
};