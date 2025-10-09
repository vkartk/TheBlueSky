import axios, { AxiosError, type InternalAxiosRequestConfig } from 'axios';
import {  AUTH_BASE_URL, REFRESH_KEY } from '@/config';
import { logout, tokensRefreshed } from '@/features/auth/authSlice';
import type { AppStore } from '@/store';

export const createAttachAuthTokenInterceptor = (store: AppStore) => {
    return (config: InternalAxiosRequestConfig) => {
        const { accessToken } = store.getState().auth;
        if (accessToken) {
            config.headers.Authorization = `Bearer ${accessToken}`;
        }
        return config;
    };
};


// State variables to manage the token refresh process
let isRefreshing = false;
let failedQueue: ((token: string) => void)[] = [];

const processQueue = (error: Error | null, token: string | null = null) => {
    failedQueue.forEach(promise => {
        if (error) {
            promise(token!);
        } else {
            promise(token!);
        }
    });
    failedQueue = [];
};

export const createHandleTokenRefreshInterceptor = (store: AppStore) => {
    return async (error: AxiosError) => {
        const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };

        if (error.response?.status === 401 && !originalRequest._retry) {
            if (isRefreshing) {
                // If a refresh is already in progress, queue the original request
                return new Promise((resolve, _reject) => {
                    failedQueue.push((token) => {
                        if (originalRequest.headers) {
                            originalRequest.headers['Authorization'] = 'Bearer ' + token;
                        }
                        resolve(axios(originalRequest));
                    });
                });
            }

            originalRequest._retry = true;
            isRefreshing = true;

            const refreshToken = localStorage.getItem(REFRESH_KEY);
            if (!refreshToken) {
                store.dispatch(logout());
                isRefreshing = false;
                return Promise.reject(error);
            }

            try {
                const { accessToken: currentAccessToken } = store.getState().auth;
                const response = await axios.post(`${AUTH_BASE_URL}/refresh-token`, {
                    accessToken: currentAccessToken,
                    refreshToken: refreshToken,
                });

                const { accessToken: newAccessToken, refreshToken: newRefreshToken } = response.data;
                store.dispatch(tokensRefreshed({ accessToken: newAccessToken, refreshToken: newRefreshToken }));
                
                // Retry the original request
                if (originalRequest.headers) {
                    originalRequest.headers['Authorization'] = `Bearer ${newAccessToken}`;
                }
                processQueue(null, newAccessToken);
                return axios(originalRequest);
            } catch (refreshError) {
                processQueue(refreshError as Error, null);
                store.dispatch(logout());
                return Promise.reject(refreshError);
            } finally {
                isRefreshing = false;
            }
        }

        return Promise.reject(error);
    };
};