import axios from 'axios';
import { BOOKINGS_BASE_URL } from '@/config';

export const bookingsApiClient = axios.create({
  baseURL: BOOKINGS_BASE_URL,
});