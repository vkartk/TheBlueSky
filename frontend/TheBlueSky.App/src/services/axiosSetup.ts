import { authApiClient } from '@/services/auth/authApiClient';
import { flightsApiClient } from '@/services/flights/flightsApiClient';
import { createAttachAuthTokenInterceptor, createHandleTokenRefreshInterceptor } from './interceptors';
import type { AppStore } from '@/store';

export const setupAxiosInterceptors = (store: AppStore) => {
    const attachAuthToken = createAttachAuthTokenInterceptor(store);
    const handleTokenRefresh = createHandleTokenRefreshInterceptor(store);

    const clients = [authApiClient, flightsApiClient];

    clients.forEach(client => {
        client.interceptors.request.use(attachAuthToken);
        client.interceptors.response.use((response) => response, handleTokenRefresh);
    });
};