import axios from 'axios';
import { ACCESS_KEY, FLIGHTS_BASE_URL } from '@/config';

export const flightsApiClient = axios.create({
  baseURL: FLIGHTS_BASE_URL,
});

flightsApiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem(ACCESS_KEY);
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);