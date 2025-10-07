import { authApiClient } from '@/services/auth/authApiClient';
import { flightsApiClient } from '@/services/flights/flightsApiClient';
import { bookingsApiClient } from '@/services/bookings/bookingsApiClient';
import { createAttachAuthTokenInterceptor, createHandleTokenRefreshInterceptor } from './interceptors';
import type { AppStore } from '@/store';

export const setupAxiosInterceptors = (store: AppStore) => {
    const attachAuthToken = createAttachAuthTokenInterceptor(store);
    const handleTokenRefresh = createHandleTokenRefreshInterceptor(store);

    const clients = [authApiClient, flightsApiClient, bookingsApiClient];

    clients.forEach(client => {
        client.interceptors.request.use(attachAuthToken);
        client.interceptors.response.use((response) => response, handleTokenRefresh);
    });
};