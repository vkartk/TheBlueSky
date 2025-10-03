import axios from 'axios';
import { AUTH_BASE_URL } from '@/config';

export const authApiClient = axios.create({
  baseURL: AUTH_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});