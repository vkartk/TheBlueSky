import axios from 'axios';
import { AUTH_BASE_URL } from '@/config';
import { store } from '@/store';

export const authApiClient = axios.create({
  baseURL: AUTH_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

authApiClient.interceptors.request.use(
  (config) => {
    const { accessToken } = store.getState().auth;
    if (accessToken) {
      config.headers.Authorization = `Bearer ${accessToken}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);