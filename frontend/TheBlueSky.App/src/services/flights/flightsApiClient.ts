import axios from 'axios';
import { FLIGHTS_BASE_URL } from '@/config';

export const flightsApiClient = axios.create({
  baseURL: FLIGHTS_BASE_URL,
});