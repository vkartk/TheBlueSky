import axios from 'axios';
import { FLIGHTS_BASE_URL } from '@/config';
import { attachAuthToken, handleTokenRefresh } from '@/services/interceptors';

export const flightsApiClient = axios.create({
  baseURL: FLIGHTS_BASE_URL,
});

flightsApiClient.interceptors.request.use(attachAuthToken);
flightsApiClient.interceptors.response.use((response) => response, handleTokenRefresh);
