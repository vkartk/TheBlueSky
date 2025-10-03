import axios from 'axios';
import { AUTH_BASE_URL } from '@/config';
import { attachAuthToken, handleTokenRefresh } from '@/services/interceptors';

export const authApiClient = axios.create({
  baseURL: AUTH_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

authApiClient.interceptors.request.use(attachAuthToken);
authApiClient.interceptors.response.use((response) => response, handleTokenRefresh);